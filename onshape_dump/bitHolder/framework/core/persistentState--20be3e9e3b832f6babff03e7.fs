//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

function new_counter() returns function
{
    
    const b = new box(0);
    return function(){b[] += 1; return toString(b[]);};
}

//const uniqueIdString is function = new_counter();
//The above statement is not allowed, because global constants are note allowed to reference boxes or builtins. (remember: No hidden state is allowed.)

// //a way to come close to the uniqueness that I want is to use a variable attached to the context
// // to store the state.
// function uniqueIdString(context is Context) returns string
// {
//     const nameOfTheVariable is string = "e2ab545c007642a0bf49475af5eaa04d"; //this is a guid that I generated specifically for this function.  I did this reather than using some maually created name like "stateForSocketHolder" in order to be reasonably confident that I will not clash with anyone else's names.
//     const defaultValue = 0; //the underlying state value will be a number, rather than a string, soi that I can increment it easily.
    
//     // if the context does not already contain a slot ('slot' here means a slot in the computer science sense, not in the mechanical sense.) for the required state information, add one.
//     var slotExists = true;
//     try silent //the 'silent' suppresses the warning message that would otherwise result.
//     {
//         getVariable(context, nameOfTheVariable); //this will throw an exception when the variable does not exist.  (For purposes of detcting whether the variable exists.I am hoping that this will only ever throw an exception when the variable does not exist.)
//     }
//     catch
//     {
//         slotExists = false;
//     }
    
//     if(!slotExists)
//     {
//          setVariable(context, nameOfTheVariable, defaultValue);   
//     }
    
//     //at this point, we are guaranteed that the slot exists.
    
//     //increment the value
//     setVariable(context, nameOfTheVariable, getVariable(context, nameOfTheVariable) + 1);
//     return toString(getVariable(context, nameOfTheVariable));
//     return "";
// }

//oops - the 'Variable(s) that setVariable and getVAriable operate on are not static between across rebuilds.  They only exist for the duration of one rebuild.
//Let's try attributes instead.
export function uniqueIdString(context is Context) returns string
{
    const myGuid = "e2ab545c007642a0bf49475af5eaa04d";
    const initialValue = 0;
    if(getSpecialValueForWholeContext(context,myGuid) is undefined)
    {
        //println("the special attribute " ~ myGuid ~ " does not already exist, so we will create a new one.");
        setSpecialValueForWholeContext(context,myGuid, initialValue); 
    } else {
        setSpecialValueForWholeContext(context,myGuid, getSpecialValueForWholeContext(context,myGuid) + 1);
    }
    
    return toString(getSpecialValueForWholeContext(context,myGuid));
}

export function uniqueId(context is Context, id is Id) returns Id
{
    return id + uniqueIdString(context);
}

type neilsSpecialAttribute typecheck is_neilsSpecialAttribute;
predicate is_neilsSpecialAttribute(x)
{
       x is map;
       x.name is string;
       
    //   !(x.value is undefined);
    //   size(x) == 2;
    //we need to relax the typecheck a bit so that we can use the type as part of the attributePattern, without having to specify a specific value (the truth is that the attributePattern syntax is limiting).
    // all of this special type nonsense is really probably unnecessary because of our use of guids.
    
        size(x) == 2 || size(x) == 1;

       //in other words, x is a map that has exactly two keys: "name" and "value".  x.name is a string, and x.value can be anything (except undefined).
}
// our special attribute shall be of the form:
//  {
//      "name":guid, 
//      "value": <<stateValue>> //stateValue is what we are really interested in.
//  }


function getSpecialValueForWholeContext(context, guid is string)
{
    var attributePattern = {"name":guid} as neilsSpecialAttribute; //will this simply match all neilsSpecialAttribute, or will the "name":guid also be considered in the search?

    var candidates = 
        evaluateQuery(
            context,
            qAttributeQuery(attributePattern)
        );
        
    if(size(candidates) == 0)
    {
        return undefined;
    } else
    {
        if (size(candidates) != 1 )
        {
            //in this case, there is more than 1 candidate entities that have one of our special attributes.  If we have done things right, this should never happen.
            //to do: issue a warning and (maybe) fix the problem by removing the excess attributes.
        }
        
        return getAttributes(context,{"entities":candidates[0],"attributePattern":attributePattern})[0].value;
    }
}

function setSpecialValueForWholeContext(context, guid is string, newValue)
{
    var attributePattern = {"name":guid} as neilsSpecialAttribute; 
     
    var newSpecialAttribute = 
        {
            "name":guid,
            "value":newValue
        }  as neilsSpecialAttribute;
    
    //first, remove any existing attributes on any entities that match attributePattern
    removeAttributes(context, {"attributePattern":attributePattern});
    
    setAttribute(context,
        {
            "entities": qNthElement(qCreatedBy(makeId("Origin")), 0), //the trick here is to return an entity that cannot ever be deleted from the context (and to have this query return a valid result from any arbitrary onShape context that we happen to be working in).  Hopefully, qCreatedBy(makeId("Origin")) will work for all parts.  (ARe there any onshape parts out there in the world (or coud there ever be any) that do not have a feature with id ["Origin"] that creates at least one entity.
            "attribute": newSpecialAttribute
        }
    );
    
    return newValue;
}

//oops. Using attributes to store persistent state is no better than setVariable/getVariable --  attributes only persist for the duration of one rebuild.  I guess attributes are still a bit better/differnt than variables in that the attributes are not accessible as '#' dereferenced values in expressions in the gui.
