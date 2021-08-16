//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "20be3e9e3b832f6babff03e7", version : "fe25cd47b70ccda1fd60e890");

export const zeroLength = 0 * meter;
export const xHat = vector(1,0,0);
export const yHat = vector(0,1,0);
export const zHat = vector(0,0,1);

export function isEven(x is number) returns boolean
{
    return (x % 2 == 0);   
}

export function isOdd(x is number) returns boolean
{
    return !isEven(x);
}

export function mean(x is array)
{
    if(size(x)==0) return undefined;
    var sum;
    sum = x[0];
    for(var i = 1; i<size(x); i+=1)
    {
        sum += x[i];
    }
    return sum / size(x);
}

// returns the element-wise product of two vectors.
export function elementWiseProduct(a is Vector, b is Vector) returns Vector
precondition
{
    size(a) == size(b);        
}
{
    var returnValue = makeArray(size(a)) as Vector;
    for(var i = 0; i<size(returnValue); i+=1)
    {
        returnValue[i] = a[i] * b[i];   
    }
    return returnValue;
}



// these implode functions are analagous to php's implode() function.
export function implode(
    glue is string, 
    pieces is array //an array of strings
) returns string
{
    var out = "";
    
    for(var i=0; i<size(pieces); i+=1) 
    {
        out ~= pieces[i] ~ ( i < size(pieces) - 1 ? glue : "" ); 
    }
    return out;
}

export function implode(
    pieces is array //an array of strings
) returns string
{
   return implode("",pieces);
}

export function str_repeat(input is string, multiplier is number) returns string
{
    return implode(makeArray(multiplier, input));
}

//analagous to php's explode() function (http://php.net/manual/en/function.explode.php).
export function explode(
    delimiter is string,
    rawString is string
) returns array
{
    if(rawString == "") {return [rawString];}
    if(delimiter == "") {return splitIntoCharacters(rawString);}
    
    var segments = [];
    
    
    //we will partition rawString into contiguous 'segments' of characters, each segment ending when we hit either the end of the string or the end of a delimeter.
    //we then remove the delimiter from the end of all but the last segment (which will not have a delimiter because it was terminated by the end of the string.
    
    var characters = splitIntoCharacters(rawString);
    var currentSegment = "";
    for(var i = 0; i < size(characters); i+=1)
    {
        currentSegment ~= characters[i];
        if(endsWith(currentSegment, delimiter))
        {
            var trimmedCurrentSegment = 
                string_slice(
                    currentSegment,
                    0,
                    length(currentSegment) - length(delimiter)  
                );
            
            // println("i: " ~ i);		// i
            // println("currentSegment: " ~ currentSegment);		// currentSegment
            // println("trimmedCurrentSegment: " ~ trimmedCurrentSegment);		// trimmedCurrentSegment
            
            segments = 
                append(
                    segments, 
                    trimmedCurrentSegment
                );
            
            currentSegment = "";
        }
        
        if(i == size(characters)-1)
        {
            //in this case, we have reached the end of rawString
            segments = append(segments, currentSegment); //note this behavior is correct even if we have just found that currentSegment ends witha delimiter and therefore reset currentSegment to the empty string  above.  Because, when currentString ends with a delimiter, that should result in the last element of the explosion being an empty string.
        }
    }
    return segments;
}

//tells us whether rawString ends with delimiter
export function endsWith(rawString is string, delimiter is string) returns boolean
{
    //return match(rawString, "^[/s/S]*" ~ escapeForRegex(delimiter) ~ "$").hasMatch;
    
    return string_slice(rawString, -length(delimiter)) == delimiter;
}

// function escapeForRegex(x is string) returns string
// {
//     // the replace() function takes a regex as the needle.  And my goal is to return an escaped regex character, so the needle and the replacement in this code are mostly going to be the same as one another.
//     x = replace(x, "\\n", "\\n"); //for now I am only going to support newlines, because that is all I need for my current project.  
    
//     return x;
// }

export function string_slice(theString is string, offset, length) returns string
{
   return implode(array_slice(splitIntoCharacters(theString),offset,length));  
}

export function string_slice(theString is string, offset is number) returns string
{
    return  string_slice(theString,offset,length(theString));
}

export function array_slice(theArray is array, offset is number, length is number) returns array
{
    if (abs(offset) > size(theArray)){return [];}   
    var firstIndex = (size(theArray) + offset) % size(theArray) ;  //this ensures that firstIndex is always in the range [0,...,size(theArray)-1], and it handles a negative offset correctly.
    //if (firstIndex < 0) {firstIndex += size(theArray);} //the stupid mod operator gives a negative result when the input is negative, but we want a positve result. (never mind)
    // print("firstIndex: " ~ firstIndex);		// firstIndex
    // print("\t");
    // println("offset: " ~ offset);		// offset
    var returnValue = [];
    for(var i=firstIndex; i<size(theArray) && size(returnValue) < length; i+=1)
    {
        returnValue = append(returnValue, theArray[i]);
    }
    
    return returnValue;
}

export function array_slice(theArray is array, offset is number) returns array
{
    return  array_slice(theArray,offset,size(theArray));
}


//returns a string that express x, rounded to the nearest 1/denominator, simplified (i.e. numerator and denominator are relatively prime).  For example toProperFractionString(10/64, 64) will return "5/32"
// TO DO handle x larger than 1.
// export function toProperFractionString(x is number, denominator is number) returns string
// precondition
// {
//     isPositiveInteger(denominator);   
// }
// {
//     var returnValue;
//     var numerator = round(x*denominator);
    
    
//     //println("numerator: " ~ toString(numerator));		// numerator
//     //println("denominator: " ~ toString(denominator));		// denominator
//     var simplifiedNumerator = numerator/greatestCommonDivisor(numerator, denominator);
//     var simplifiedDenominator = denominator/greatestCommonDivisor(numerator, denominator);
//     returnValue = toString(simplifiedNumerator) ~ "/" ~ toString(simplifiedDenominator);
    
//     // 1/7   : "\u2150"
//     // 1/9   : "\u2151"
//     // 1/10  : "\u2152"
//     // 1/3   : "\u2153"
//     // 2/3   : "\u2154"
//     // 1/5   : "\u2155"
//     // 2/5   : "\u2156"
//     // 3/5   : "\u2157"
//     // 4/5   : "\u2158"
//     // 1/6   : "\u2159"
//     // 5/6   : "\u215a"
//     // 1/8   : "\u215b"
//     // 3/8   : "\u215c"
//     // 5/8   : "\u215d"
//     // 7/8   : "\u215e"
//     // 1/    : "\u215f"
//     // 1/4   : "\u00bc"
//     // 1/2   : "\u00bd"
//     // 3/4   : "\u00be"
    
//     //attempt to compute unicodeReturnValue, which uses some of the special unicode codepoints for fractions.
//     var unicodeReturnValue = undefined;
//          if( returnValue == "1/7"  ) {unicodeReturnValue = "\u2150";}
//     else if( returnValue == "1/9"  ) {unicodeReturnValue = "\u2151";}
//     else if( returnValue == "1/10" ) {unicodeReturnValue = "\u2152";}
//     else if( returnValue == "1/3"  ) {unicodeReturnValue = "\u2153";}
//     else if( returnValue == "2/3"  ) {unicodeReturnValue = "\u2154";}
//     else if( returnValue == "1/5"  ) {unicodeReturnValue = "\u2155";}
//     else if( returnValue == "2/5"  ) {unicodeReturnValue = "\u2156";}
//     else if( returnValue == "3/5"  ) {unicodeReturnValue = "\u2157";}
//     else if( returnValue == "4/5"  ) {unicodeReturnValue = "\u2158";}
//     else if( returnValue == "1/6"  ) {unicodeReturnValue = "\u2159";}
//     else if( returnValue == "5/6"  ) {unicodeReturnValue = "\u215a";}
//     else if( returnValue == "1/8"  ) {unicodeReturnValue = "\u215b";}
//     else if( returnValue == "3/8"  ) {unicodeReturnValue = "\u215c";}
//     else if( returnValue == "5/8"  ) {unicodeReturnValue = "\u215d";}
//     else if( returnValue == "7/8"  ) {unicodeReturnValue = "\u215e";}
//     else if( returnValue == "1/4"  ) {unicodeReturnValue = "\u00bc";}
//     else if( returnValue == "1/2"  ) {unicodeReturnValue = "\u00bd";}
//     else if( returnValue == "3/4"  ) {unicodeReturnValue = "\u00be";}
//     else
//     {
//         unicodeReturnValue = 
//             (
//                 simplifiedNumerator == 1 
//                 ? 
//                 "\u215f" //this is the code point for a one and a slash (pronounce "one over")
//                 : 
//                 implode(
//                     mapArray(
//                         splitIntoCharacters(toString(simplifiedNumerator)),
//                         function(x){
//                             //superscripts
//                             return
//                                 {
//                                   "0" : "\u2070",
//                                   "1" : "\u00b9",
//                                   "2" : "\u00b2",
//                                   "3" : "\u00b3",
//                                   "4" : "\u2074",
//                                   "5" : "\u2075",
//                                   "6" : "\u2076",
//                                   "7" : "\u2077",
//                                   "8" : "\u2078",
//                                   "9" : "\u2079"
//                                 }[x];
//                         }
//                     )
//                 ) ~ "/" //"\u2044" // codepoint u+2044 is the fraction slash (strangely, this is causing nothing to be displayed in the galley (perhaps this character pushes everything else far to one side?
//             ) 
//             ~
//             // implode(
//             //     mapArray(
//             //         splitIntoCharacters(toString(simplifiedDenominator)),
//             //         function(x){
//             //             //subscripts
//             //             return
//             //                 {
//             //                   "0" : "\u2080",
//             //                   "1" : "\u2081",
//             //                   "2" : "\u2082",
//             //                   "3" : "\u2083",
//             //                   "4" : "\u2084",
//             //                   "5" : "\u2085",
//             //                   "6" : "\u2086",
//             //                   "7" : "\u2087",
//             //                   "8" : "\u2088",
//             //                   "9" : "\u2089"
//             //                 }[x];
//             //         }
//             //     )
//             // );
//             // it looks like none of the onshape fonts contain characters for the subscripts (they just show up as boxes)
//             toString(simplifiedDenominator);
//     }



//     if (! (unicodeReturnValue is undefined) )
//     {
//         return unicodeReturnValue;   
//     } else { 
//         return returnValue;
//     }
// }
// I would like to use unicode superscripts and subscripts, but the onshape fonts do not seem to support unicode subscript characters. 



export function toProperFractionString(x is number, denominator is number) returns string
precondition
{
    isPositiveInteger(denominator);   
}
{
    var returnValue;
    var numerator = round(x*denominator);
    
    
    //println("numerator: " ~ toString(numerator));		// numerator
    //println("denominator: " ~ toString(denominator));		// denominator
    var simplifiedNumerator = numerator/greatestCommonDivisor(numerator, denominator);
    var simplifiedDenominator = denominator/greatestCommonDivisor(numerator, denominator);
    returnValue = toString(simplifiedNumerator) ~ "/" ~ toString(simplifiedDenominator);
    
    // 1/7   : "\u2150"
    // 1/9   : "\u2151"
    // 1/10  : "\u2152"
    // 1/3   : "\u2153"
    // 2/3   : "\u2154"
    // 1/5   : "\u2155"
    // 2/5   : "\u2156"
    // 3/5   : "\u2157"
    // 4/5   : "\u2158"
    // 1/6   : "\u2159"
    // 5/6   : "\u215a"
    // 1/8   : "\u215b"
    // 3/8   : "\u215c"
    // 5/8   : "\u215d"
    // 7/8   : "\u215e"
    // 1/    : "\u215f"
    // 1/4   : "\u00bc"
    // 1/2   : "\u00bd"
    // 3/4   : "\u00be"
    
    //attempt to compute unicodeReturnValue, which uses some of the special unicode codepoints for fractions.
    var unicodeReturnValue = undefined;
         if( returnValue == "1/7"  ) {unicodeReturnValue = "\u2150";}
    else if( returnValue == "1/9"  ) {unicodeReturnValue = "\u2151";}
    else if( returnValue == "1/10" ) {unicodeReturnValue = "\u2152";}
    else if( returnValue == "1/3"  ) {unicodeReturnValue = "\u2153";}
    else if( returnValue == "2/3"  ) {unicodeReturnValue = "\u2154";}
    else if( returnValue == "1/5"  ) {unicodeReturnValue = "\u2155";}
    else if( returnValue == "2/5"  ) {unicodeReturnValue = "\u2156";}
    else if( returnValue == "3/5"  ) {unicodeReturnValue = "\u2157";}
    else if( returnValue == "4/5"  ) {unicodeReturnValue = "\u2158";}
    else if( returnValue == "1/6"  ) {unicodeReturnValue = "\u2159";}
    else if( returnValue == "5/6"  ) {unicodeReturnValue = "\u215a";}
    else if( returnValue == "1/8"  ) {unicodeReturnValue = "\u215b";}
    else if( returnValue == "3/8"  ) {unicodeReturnValue = "\u215c";}
    else if( returnValue == "5/8"  ) {unicodeReturnValue = "\u215d";}
    else if( returnValue == "7/8"  ) {unicodeReturnValue = "\u215e";}
    else if( returnValue == "1/4"  ) {unicodeReturnValue = "\u00bc";}
    else if( returnValue == "1/2"  ) {unicodeReturnValue = "\u00bd";}
    else if( returnValue == "3/4"  ) {unicodeReturnValue = "\u00be";}
    return returnValue;         
    if (! (unicodeReturnValue is undefined) )
    {
        return unicodeReturnValue;   
    } else { 
        return returnValue;
    }
}

export function greatestCommonDivisor(a is number, b is number) returns number
precondition
{
    isNonNegativeInteger(a);    
    isNonNegativeInteger(b); 
}
{
      // Euclidean algorithm
      var remainders = [max(a,b), min(a,b)];
      if(remainders[0]==0){return 0;}
      var k;
      for(k=0; k < 10000; k+=1) //the "k<10000" is a an infinite-loop preventer, in case I have screwed something up.
      {
          if(k>=2){
            remainders = append(remainders, remainders[k-2] % remainders[k-1]);
          }
          //println("remainders[k]: " ~ toString(remainders[k]));		// remainders[k]
          if(remainders[k] == 0){break;}
      }
      return remainders[k-1];
}

//these overload of the max and min functions return the value in the array which yields the maximum and minimum metric, respectively.  (metric is a function which takes an array element as an argument and returns a real number.)
export function max(theArray is array, metric is function)
{
    if(size(theArray) == 0){return undefined;}
    
    return 
        theArray[
            argMax(
                mapArray(
                    theArray,
                    metric
                )
            )
        ];
}

export function min(theArray is array, metric is function)
{
    return max(theArray, function(x){return -metric(x);});
}


export function createCircleSheetBody(context is Context, id is Id, definition is map) returns Query
precondition
{
    definition.circle is Circle;   
}
{
    var idOfWorkingSketch = uniqueId(context, id);
    
    var workingSketch = newSketchOnPlane(context, idOfWorkingSketch, {sketchPlane: plane(definition.circle.coordSystem)});
    
    
    skCircle(workingSketch, uniqueIdString(context), {center: vector(zeroLength, zeroLength), radius: definition.circle.radius});
    skSolve(workingSketch);
    
    var sheetBody = qBodyType(qCreatedBy(idOfWorkingSketch, EntityType.BODY), BodyType.SHEET);
    
    
    var bodiesToDelete = qSubtraction( qCreatedBy(idOfWorkingSketch, EntityType.BODY), sheetBody);
    
    //print("bodiesToDelete: "); debug(context, bodiesToDelete);
    
    //println("reportOnEntities(context, bodiesToDelete, 0, 0 ): " ~ toString(reportOnEntities(context, bodiesToDelete, 0, 0 )));		// reportOnEntities(context, bodiesToDelete, 0 );
    
    try  {
        opDeleteBodies(context, uniqueId(context, id), {entities:bodiesToDelete}); //delete everything left over from the sketch Except the sheet body that we want to return.
    }
    //print("bodiesToDelete after deletion: "); debug(context, bodiesToDelete);
    return sheetBody;
}

//creates a planar sheet body (on the xy plane of the specified definition.coordSystem whose boundary
// is a polygon defined by the specified definition.vertices
export function createPolygonalSheetBody(context is Context, id is Id, definition is map) returns Query
precondition
{
    definition.coordSystem is CoordSystem;
    is2dPointVector(definition.vertices); 
    //in other words, definition.outlineVertices is expected to be an array where each member is a 2-element vector of lengths.
}
{
    var idOfWorkingSketch = uniqueId(context, id);
    
    var workingSketch = newSketchOnPlane(context, idOfWorkingSketch, {sketchPlane: plane(definition.coordSystem)});
    
    //make sure the list of vertices is closed (last and first are same)
    if(!tolerantEquals(definition.vertices[0], definition.vertices[size(definition.vertices) - 1]))
    {
        definition.vertices = append(definition.vertices, definition.vertices[0]);    
    }
    
    skPolyline(workingSketch, "polyline",
        {
            points:definition.vertices 
        }
    );

    skSolve(workingSketch);

    var sheetBody = qBodyType(qCreatedBy(idOfWorkingSketch, EntityType.BODY), BodyType.SHEET);
    
    //we need to delete all of the edge bodies, point bodies, etc. created by the sketch -- everything except our sheet body.
    var bodiesToDelete = qSubtraction( qCreatedBy(idOfWorkingSketch, EntityType.BODY), sheetBody);
        
    try  {
        opDeleteBodies(context, uniqueId(context, id), {entities:bodiesToDelete}); //delete everything left over from the sketch Except the sheet body that we want to return.
    }
    return sheetBody;
}

export function createRightPolygonalPrism(context is Context, id is Id, definition is map) returns Query
precondition
{
    definition.plane is Plane;
    
    definition.vertices is array;
    for(var vertex in definition.vertices){ 
        //is2dPoint(vertex) || is3dLengthVector; //if 3d, we will project the points onto the plane (not yet implemented)
        is2dPoint(vertex);
    }
    definition.height is ValueWithUnits ;
}
{
    
    
    
    
    //var idOfWorkingSketch = uniqueId(context, id);
    //
    //var workingSketch = newSketchOnPlane(context, idOfWorkingSketch, {sketchPlane: definition.plane});
    //
    ////TO DO// project any 3d vertices onto the plane, to ensure that each element of definition.vertices is a 2d vector.
    //
    ////make sure the list of vertices is closed (last and first are same)
    //if(!tolerantEquals(definition.vertices[0], definition.vertices[size(definition.vertices) - 1]))
    //{
    //    definition.vertices = append(definition.vertices, definition.vertices[0]);    
    //}
    //
    //skPolyline(workingSketch, "polyline",
    //    {
    //        points:definition.vertices 
    //    }
    //);
    //
    //skSolve(workingSketch);
    //
    //var sheetBody = qBodyType(qCreatedBy(idOfWorkingSketch, EntityType.BODY), BodyType.SHEET);
    //
    //var bodiesToDelete = qSubtraction( qCreatedBy(idOfWorkingSketch, EntityType.BODY), sheetBody);  // we will delete all bodies created by workingSketch except sheetBody.
    
    
    
    
    var sheetBody = createPolygonalSheetBody(
        context,
        uniqueId(context, id),
        {
            "coordSystem": coordSystem(definition.plane),
            "vertices": definition.vertices
        }
    );
    var bodiesToDelete = sheetBody;
    
    
    var idOfExtrude = uniqueId(context, id);
    //annotation { "Part Name" : "ahoy" }
    opExtrude(
        context,
        idOfExtrude, 
        {
            "entities" : qOwnedByBody(sheetBody, EntityType.FACE),
            "direction" : evOwnerSketchPlane(context, {"entity" : sheetBody}).normal,
            "endBound" : BoundingType.BLIND,
            "endDepth" : definition.height/2,
            "startBound" : BoundingType.BLIND,
            "startDepth" : definition.height/2
        }
    );
    
    var returnValue = qBodyType(qCreatedBy(idOfExtrude, EntityType.BODY),BodyType.SOLID);
    try  {
        opDeleteBodies(context, uniqueId(context, id), {entities:bodiesToDelete}); 
    }

    return returnValue;
}

//overload of floor() to implement the two argument behavior, where you can specify a modulus other than 1.    
//export function floor(x, modulus)
//{
//    return floor(x/modulus) * modulus; 
//}
// It appears that between Featurescript versions xxx and 1271, OnShape got their act together
// and implemented the two-argument version of the floor function, so this custom overload is no longer needed.
//Bizarrely, it seems that having my custom version of the two-argumnet floor function in place caused the opBoolean 
// that performs the clipping of text bodies in the galley class to fail, even though the galley class doesn't use the floor function anywhere.

//returns an array containing two points, which are the extreme points of the region of the axis that intersects the bodies.
// the first element of the array is the extreme point in the negative direction of the axis, and the 
// the second element of the array is the extreme point in the positive direction of the axis.
export function evExtremeSkewerPointsOfBodies(context is Context, bodies is Query, axis is Line) returns array
{
    //axis is a Line (i.e. position and direction)

    // we will use qIntersectsLine() to get all faces of the bodies that intersect the line, and then we will use evDistance() to get the intersection points.
    
    var candidateFaces = evaluateQuery(context,qIntersectsLine(qOwnedByBody(bodies, EntityType.FACE), axis));
    
   // debug(context, axis);
   // debug(context, qUnion(candidateFaces));
    
    var intersectionPoints = 
        mapArray(
            candidateFaces,
            function(face is Query)
            {
                //get the intersection point of face with axis
                var distanceResult = 
                    evDistance(
                        context,
                        {
                            side0: axis,
                            side1: face
                        }
                    );
                
                if(!tolerantEquals(distanceResult.distance, zeroLength))
                {
                    // reportFeatureInfo(
                    //     context, 
                    //     id, 
                    println(
                        "Something strange has happened when finding intersection points between a face and a line: "     ~ 
                        "we expected, based on the results of qIntersectsLine(), that the face and the line intersect, "  ~ 
                        "and yet evDistance() returns a non-zero distance (" ~ toString(distanceResult.distance) ~ ") "   ~
                        "between the face and the line." 
                    );   
                }
                
                return distanceResult.sides[0].point;
            }
        );
        
    
        
    
    var metric = 
        function(x)
        {
            return 
                dot(
                    axis.direction,
                    x - axis.origin
                );
        };
        
    return 
        [
            min(intersectionPoints,metric), 
            max(intersectionPoints,metric)
        ];
         
}


//this gives access to members that are already public via get_...() and set_...() functions, for convenience and consistency of syntax.
export function addDefaultGettersAndSetters(this is box)
{
    for(var propertyName in keys(this[]))  
    {
        if(! (this[][propertyName] is function))
        {
            
            if(this[]["get_" ~ propertyName] == undefined) //if there is not already a property named ("get_" ~ propertyName)
            {
                this[]["get_" ~ propertyName] = function(){return this[][propertyName];};
            }
            
            if(this[]["set_" ~ propertyName] == undefined) //if there is not already a property named ("set_" ~ propertyName)
            {
                this[]["set_" ~ propertyName] = function(newValue){this[][propertyName] = newValue; return newValue;};
            }
        }
    }
    
    //this is a setter function that takes a map and attempts to run all the set_x() functions of this[], where x is a key in the map.
    this[].set = 
        function(settings is map)
        {
            for(var setting in settings)
            {
                if(
                    !(setting.value is function)
                    && isIn("set_" ~ setting.key, keys(this[]))
                    && (this[]["set_" ~ setting.key] is function)
                )
                {
                    this[]["set_" ~ setting.key](setting.value);
                }  
            }
        };
}


//converts a map into a hierarchically-indented multi-line string, that 
// extends to the deepest nesting level.  This will technically work for
// maps whose keys are maps and strings, but the output probably wouldn't be very 
// pretty.  This is intended to work with maps each of whose keys is of a type that
// will, when operated on with toString(), return a short string.  Good types for 
// a key would be number, string, and boolean.

export function mapToString(x is map) returns string
{
    return mapToString(x, 0) ;
}

export function mapToString(x is map, tabLevel is number) returns string
{
    var out is string = "";
    for(var item in x)
    {
        out ~=  str_repeat("\t", tabLevel) ~ item.key ~ ": ";
        if(item.value is map)
        {
            //drop down a line and then re-enter, with tabLevel increased by one.
            out ~= "\n";
            out ~= mapToString(item.value, tabLevel + 1);   
        } else 
        {
            out ~= toString(item.value) ~ "\n";
        }
    }
    
    return out;
}

