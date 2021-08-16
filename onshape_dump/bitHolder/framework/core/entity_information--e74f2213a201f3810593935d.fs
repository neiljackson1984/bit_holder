//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");

//this function returns the type of the first entity returned by the query (or undefined if we fail to identify an entityType)
//export function getEntityType(context is Context, q is Query)  returns EntityType //we cannot declare a specific return type because sometimes we will return EntityType and sometimes we will return undefined.
export function getEntityType(context is Context, q is Query) precondition{queryReturnsExactlyOneEntity(context,q);}
{   
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    
    for(var thisEntityType in values(EntityType))
    {
        if(queryReturnsSomething(context, qEntityFilter(q, thisEntityType)))
        {
            return thisEntityType as EntityType;
        }
    }
    //if execution makes it this far, it means we have not identified an EntityType, likely because the query did not return anything.
    return undefined;
}

export function isVertex(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  getEntityType(context, q) == EntityType.VERTEX;  
}

export function isEdge(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  getEntityType(context, q) == EntityType.EDGE;  
}

export function isFace(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  getEntityType(context, q) == EntityType.FACE;  
}

export function isBody(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  getEntityType(context, q) == EntityType.BODY;  
}




//all entities have a body type.  Even an entity that is not itself a body (for instance: an edge) has a bodyType.  In the case of an edge, I think that the bodyType would be the bodyType of the body that owns the edge.  What happens in the case where an edge is owned by a body of type SOLID and is also owned by a body of type SHEET?  (Maybe I am confused on this point, perhaps an edge cannot be "owned by" more than one body.  (that would actually make sense becuase the common-sense concept of ownership is one-to-many (one owner owns many things.  Each thing is owned by (at most) one owner).
//Because even entities whose entityType is not body nevertheless have a BodyType, it makes sense, for convenience and clarity, it makes sense to have a separate function for each of the two cases (either the entity has, or does not have, entityType=BODY).

//this function gets the body type of any entity (including an entity that has entityType!=BODY)
export function getBodyTypeOfEntity(context is Context, q is Query)  precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    for(
        var thisBodyType in 
        filter(
            values(BodyType),
            function(x){return x != BodyType.MATE_CONNECTOR;} // I think that the MATE_CONNECTOR BodyType is a dummy value that is only used in the query filtering specification for the UI (THAT'S SLOPPY, onShape!).  I was finding that a body would be returned by qBodyType(q, BodyType.MATE_CONNECTOR) no matter what the actual body type was (wireBody, sheetBody, solidBody, or pointBody are the true body types) so I am ignoring the MATE_CONNECTOR value of the BodyType enum.
        )
    )
    {
        if(queryReturnsSomething(context, qBodyType(q, thisBodyType)))
        {
            return thisBodyType as BodyType;
        }
    }
    //if execution makes it this far, it means we have not identified a BodyType
    return undefined;
}

//this function simply calls getBodyTypeOfEntity(), but this function has the precondition that the entityType must be a body.
//This will help the developer to notice any fringe cases where he gets a non-body where he had assumed that he would always get a body
export function getBodyTypeOfBody(context is Context, q is Query)  precondition{queryReturnsExactlyOneEntity(context,q);  isBody(context, q);}
{
    
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    //if(!isBody(context, q)) throw "We were expecting the entity to be a body, but instead, it was of type " ~ getEntityType(context,q);
	
    return getBodyTypeOfEntity(context, q);
}



export function isSolidBody(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  isBody(context,q) && (getBodyTypeOfEntity(context, q) == BodyType.SOLID);  
}

export function isSheetBody(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  isBody(context,q) && (getBodyTypeOfEntity(context, q) == BodyType.SHEET);  
}

export function isWireBody(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  isBody(context,q) && (getBodyTypeOfEntity(context, q) == BodyType.WIRE);  
}

export function isPointBody(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  isBody(context,q) && (getBodyTypeOfEntity(context, q) == BodyType.POINT);  
}

export function isMateConnectorBody(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return  isBody(context,q) && (getBodyTypeOfEntity(context, q) == BodyType.MATE_CONNECTOR);  
}


//this function generates a string that is a nicely formatted huiman-readable 
// summary of each of the entitities returned by the Query q, and the constituent entities 
// of those entities, recursively.

export function reportOnEntity(context is Context, q is Query, tabLevel is number, recursionDepth is number) returns string//  precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    var out is string = "";
    
    out ~= str_repeat("\t",tabLevel) ~ "EntityType: " ~ getEntityType(context,q) ~ "\n";
    out ~= str_repeat("\t",tabLevel) ~ "BodyType: " ~ getBodyTypeOfEntity(context,q) ~ "\n";
    out ~= str_repeat("\t",tabLevel) ~ "isSketchEntity: " ~ isSketchEntity(context,q) ~ "\n";
    out ~= str_repeat("\t",tabLevel) ~ "isConstructionObject: " ~ isConstructionObject(context,q) ~ "\n";
    out ~= str_repeat("\t",tabLevel) ~ "ownsItself: " ~ ownsItself(context,q) ~ "\n";
    out ~= str_repeat("\t",tabLevel) ~ "number of owning bodies: " ~ getNumberOfOwningBodies(context,q) ~ "\n";
    
    var ownedEntitiesExcludingItself = qSubtraction( qOwnedByBody(q), q);
    //it seems that each body owns itself, which causes an infinite loop in our recursion down the ownership tree, so we have to work with the owned entities Other than the body itself.
    // if( hasOwnedEntities(context,q)   ||  isBody(context,q))  //I suspect that qOwnedByBody() will only return anything if the argument is a body, but it doens't hurt to do this for all type of entity.  The right-hand side of the OR statement above will mean that we will always report on the owned entities of a body, even if there are none, because I want to be able to identify the unusual case where a body has no owned entities (I am not sure such a thing can happen, but I want to notice it if it ever does.)
    if( isBody(context,q) || queryReturnsSomething(context, ownedEntitiesExcludingItself) ) 
    {
        
        out ~= str_repeat("\t",tabLevel) ~ "has " ~ numberOfEntitiesReturnedByQuery(context, ownedEntitiesExcludingItself) ~ " owned entities" ~ (ownsItself(context,q) ? " (excluding itself)" : "") ~ ": ";
        if(recursionDepth > 0)
        {
            out ~= reportOnEntities(context, ownedEntitiesExcludingItself, tabLevel + 1, recursionDepth - 1);
        } else
        {
            // in this case, we will not recurse, but we will state the number of  ownedEntitiesExcludingItself and the number of each entityType among ownedEntitiesExcludingItself.
           
            for(var i = 0; i < size(values(EntityType)); i+=1)
            {
                var thisEntityType = values(EntityType)[i];
                var isLast = (i==size(values(EntityType))-1);
                var isPenultimate = (i==size(values(EntityType))-2);
                var numberOfThisEntityType = numberOfEntitiesReturnedByQuery(context,  qEntityFilter(ownedEntitiesExcludingItself, thisEntityType) );
                
                //apply terminal 's' as appropriate to achieve proper grammatical number, and add commas and an "and" before the last element.
                out ~= numberOfThisEntityType ~ " " ~ thisEntityType ~ (numberOfThisEntityType == 1 ? "" : "s") ~ (!isLast ? ", " ~ (isPenultimate ? "and " : "") : "."); 
            }
            out ~= "\n";
                
        }
    }
    return out;
}

export function reportOnEntities(context is Context, q is Query, tabLevel is number, recursionDepth is number) returns string
{
    if(!queryReturnsSomething(context, q))
    {
        return str_repeat("\t",tabLevel) ~ "(The query did not return any entities.)" ~ "\n";
    }
    
    var out is string = "";
    var i =0;
    for(var r in evaluateQuery(context, q))
    {
        i +=1;
        out ~= str_repeat("\t",tabLevel) ~ "entity " ~ i ~ " of " ~ numberOfEntitiesReturnedByQuery(context, q) ~ ": " ~ "\n";
        out ~= reportOnEntity(context, r,tabLevel+1, recursionDepth);
    }
    return out;
}

//overload to provide a default recursionDepth of inf.
export function reportOnEntities(context is Context, q is Query, tabLevel is number) returns string {return reportOnEntities(context, q, tabLevel, inf);}

//overload to provide a default tabLevel of zero and default recursion depth of inf.
export function reportOnEntities(context is Context, q is Query) returns string {return reportOnEntities(context, q, 0, inf);}


//overload to provide a default recursionDepth of inf.
export function reportOnEntity(context is Context, q is Query, tabLevel is number) returns string {return reportOnEntity(context, q, tabLevel, inf);}

//overload to provide a default tabLevel of zero and default recursion depth of inf.
export function reportOnEntity(context is Context, q is Query) returns string {return reportOnEntity(context, q, 0, inf);}


export function isSketchEntity(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return queryReturnsSomething(context, qSketchFilter(q, SketchObject.YES));
}

export function isConstructionObject(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return queryReturnsSomething(context, qConstructionFilter (q, ConstructionObject.YES));
}

// this function tells us whether the entity is owned by itself.  (I suspect, based on an infinite recursion that I encountered while developing my recursive body information funtions, that each body owns itself.
export function ownsItself(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    
    if (   queryReturnsSomething(context, qIntersection([q, qOwnedByBody(q)])) != queryReturnsSomething(context, qIntersection([q, qOwnerBody(q)])) ) //this will probably will never happen, but I want to know about it if it does happen.
    {
        throw 
            "A very bizarre situation has occured: we have encountered an entity that both " ~
            (queryReturnsSomething(context, qIntersection([q, qOwnedByBody(q)])) ? "is owned by itself" : "is not owned by itself") ~
            " and " ~ 
           (queryReturnsSomething(context, qIntersection([q, qOwnerBody(q)])) ? "owns itself" : "does not own itself") ~ 
           ".  Isn't that weird?" ~
           "";
    }
    
    return queryReturnsSomething(context, qIntersection([q, qOwnedByBody(q)]));
}



export function hasOwnedEntities(context is Context, q is Query) returns boolean precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return getNumberOfOwnedEntities(context, q) > 0;  
}


export function getNumberOfOwningBodies(context is Context, q is Query) returns number precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return numberOfEntitiesReturnedByQuery(context, qOwnerBody(q));  
}

export function getNumberOfOwnedEntities(context is Context, q is Query) returns number precondition{queryReturnsExactlyOneEntity(context,q);}
{
    // throwExceptionIfNot_queryReturnsExactlyOneEntity(context,q);
    return numberOfEntitiesReturnedByQuery(context, qOwnedByBody(q));  
}

export function numberOfEntitiesReturnedByQuery(context is Context, q is Query) returns number
{
    
    return size(evaluateQuery(context,q));
}

export function queryReturnsSomething(context is Context, q is Query) returns boolean
{
    return numberOfEntitiesReturnedByQuery(context,q) > 0;
}

export function queryReturnsExactlyOneEntity(context is Context, q is Query) returns boolean
{
    return numberOfEntitiesReturnedByQuery(context,q) == 1;
}

function throwExceptionIfNot_queryReturnsExactlyOneEntity(context is Context, q is Query)
{
    if(!queryReturnsExactlyOneEntity(context,q)) throw "We were expecting queryReturnsExactlyOneEntity, but instead, numberOfEntitiesReturnedByQuery() is " ~ numberOfEntitiesReturnedByQuery(context,q) ~ ".";
}
