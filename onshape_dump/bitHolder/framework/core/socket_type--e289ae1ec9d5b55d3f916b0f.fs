//FeatureScript 638;
//import(path : "onshape/std/geometry.fs", version : "638.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");
export import(path : "7718ed3c3facd33626142ef1", version : "51ca12e94c2f5e082eadb4cd");


export type socket typecheck isSocket;

export predicate isSocket(value)
{
    value is box; 
}
//a scoket is a specialized type of (i.e. inherits from) bit.
//the only thing that socket does differently is that it overrides the preferredLabelText property, and defines a couple of other properties to help
// specify preferredLabelText.
export function new_socket() returns socket
{
    var this = new_bit(); //inherits from bit
    this[].nominalSize = 3/8 * inch;
    this[].nominalUnits = "inch"; //can be any key of the map STRING_TO_UNIT_MAP defined in onshape/std/units.fs
    this[].driveSize = 1/2 * inch; //this is the size of the square nub
    this[].explicitLabelText = false;
    
    
    this[].get_preferredLabelText = 
        function()
        {
            if(this[].get_explicitLabelText() is string)
            {
                return this[].get_explicitLabelText();   
            } else
            {
                var unitlessSize = this[].get_nominalSize()/STRING_TO_UNIT_MAP[this[].get_nominalUnits()];
                if(this[].get_nominalUnits() == "inch")
                {
                    return toProperFractionString(unitlessSize, 64) ~ "\n" ~ "in";   
                } else if(this[].get_nominalUnits() == "millimeter")
                {
                    unitlessSize = round(unitlessSize*10)/10;
                    return toString(unitlessSize) ~ "\n" ~ "mm";   
                }
            }
        };
    
    addDefaultGettersAndSetters(this);//this gives access to members that are already public via get_...() and set_...() functions, for convenience and consistency of syntax.
    
    return this as socket;
}


export function new_socket(initialSettings) returns socket
{
    var this = new_socket();
    this[].set(initialSettings);
    return this as socket;
}


