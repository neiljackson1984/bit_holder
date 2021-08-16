//FeatureScript 638;
//import(path : "onshape/std/geometry.fs", version : "638.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");

//the 'bit' type is a simply a bundle of properties describing a bit.
export type bit typecheck isBit;

export predicate isBit(value)
{
    value is box; 
                
    annotation { "Name" : "outerDiameter" }
    isLength(value[].get_outerDiameter(), LENGTH_BOUNDS);

    annotation { "Name" : "preferredLabelText" }
    value[].get_preferredLabelText() is string;
}


//a bit is essentially a cylinder with a preferredLabelText property.
export function new_bit() returns bit
{
    var this = new box({});
    this[].outerDiameter = 17 * millimeter;
    this[].length = 25 * millimeter;
    this[].preferredLabelText = "DEFAULT";

    
    addDefaultGettersAndSetters(this);//this gives access to members that are already public via get_...() and set_...() functions, for convenience and consistency of syntax.
    
    return this as bit;
}


export function new_bit(initialSettings) returns bit
{
    var this = new_bit();
    this[].set(initialSettings);
    return this as bit;
}

//asdfasdfasdf
export predicate uiForBitSpec(y)
{
 println("y: " ~ toString(y));		// 
   annotation { "Name" : "outerDiameter" }       
   isLength(y.outerDiameter, LENGTH_BOUNDS);

   //annotation { "Name" : "preferredLabelText" }
   //y.preferredLabelText is string;
}

