//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");
FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "20be3e9e3b832f6babff03e7", version : "fe25cd47b70ccda1fd60e890");
//import(path : "e74f2213a201f3810593935d", version : "64c079e8e4d0a2c9ff16db51");
//import(path : "ec66a324d53ae9ba8ca8980b", version : "959ee76dc517aee7424c3435");
import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");
import(path : "32a36aeb644ab3b27938129d", version : "b888eae22a7eb009fa174634");
import(path : "7718ed3c3facd33626142ef1", version : "51ca12e94c2f5e082eadb4cd");
import(path : "e289ae1ec9d5b55d3f916b0f", version : "a682a8251a389f5e7f1509e9");


    
export function getCannedDesigns(){
    

    var predefinedBitHolders = {};
    var nameOfThisBitHolder = "";
    
    nameOfThisBitHolder = "bondhus_hex_drivers_holder"; {   
        var bits = [];        
        {  //define bits
            bits = [];
            
            var defaultBit = new_socket({
                "driveSize":           999.123456 * inch,
                "length":              76.5 * millimeter,
                "outerDiameter":       (1/4) * inch * 1/cos(30*degree), //circumcscribed circle diameter of regular hexagon having inscribed circle diameter 1/4 inch.
                "nominalUnits":        "millimeter"
            });
            
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         2 * millimeter
            })));
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         2.5 * millimeter
            })));
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         3 * millimeter
            })));
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         4 * millimeter
            })));
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         5 * millimeter
            })));
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         6 * millimeter
            })));
            //defaultBit[].set_length(29.24 * millimeter);
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         8 * millimeter,
                "outerDiameter":       8 * millimeter * 1/cos(30*degree)
            })));
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         10 * millimeter,
                "outerDiameter":       10 * millimeter * 1/cos(30*degree)
            })));
            bits = append(bits, new_socket(mergeMaps(defaultBit[],{
                "nominalSize":         12 * millimeter,
                "outerDiameter":       12 * millimeter * 1/cos(30*degree)
            })));
        }   
        
        // defaultBitHolderSegment stores the settings common to all the segments in this bitHolder
        var defaultBitHolderSegment = 
            new_bitHolderSegment({
                labelFontHeight: [4.75 * millimeter,3.2*millimeter],
                lecternAngle: 70*degree
            });
         
        var bitHolder = 
            new_bitHolder(
                {
                    "segments": 
                        mapArray(
                            bits, 
                            function(theBit)
                            {
                                var theBitHolderSegment = new_bitHolderSegment(defaultBitHolderSegment[]); 
                                theBitHolderSegment[].set_bit(theBit); 
                                theBitHolderSegment[].set_bitProtrusion(
                                    theBitHolderSegment[].get_bit()[].get_length() - 18*millimeter
                                );
                                theBitHolderSegment[].set_angleOfElevation(theBitHolderSegment[].get_lecternAngle()); 
                                return theBitHolderSegment;
                            }
                        ),
                   "mountHolesPositionZSpecifier":-10 * millimeter
                }
            );
        predefinedBitHolders[nameOfThisBitHolder] = bitHolder;
    }
    nameOfThisBitHolder = "3/8-inch drive, metric sockets holder"; {   
        var sockets =[];
        var defaultSocket = new_socket({
            "driveSize":           3/8 * inch,
            "length":              25.96 * millimeter,
            "nominalUnits":        "millimeter"
        });
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         10 * millimeter,
            "outerDiameter":       17.18 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         11 * millimeter,
            "outerDiameter":       17.18 * millimeter 
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         12 * millimeter,
            "outerDiameter":       17.8 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         13 * millimeter,
            "outerDiameter":       18.3 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         14 * millimeter,
            "outerDiameter":       19.71 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         15 * millimeter,
            "outerDiameter":       20.43 * millimeter
        })));
        defaultSocket[].set_length(29.24 * millimeter);
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         17 * millimeter,
            "outerDiameter":       23.36 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         19 * millimeter,
            "outerDiameter":       25.87 * millimeter
        })));
        predefinedBitHolders[nameOfThisBitHolder] = socketHolder(sockets);
    }
    nameOfThisBitHolder = "3/8-inch drive, imperial sockets holder"; {   
        var sockets =[];
        var defaultSocket = new_socket({
            "driveSize":           3/8 * inch,
            "length":              25.96 * millimeter,
            "nominalUnits":        "inch"
        });
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         3/8 * inch,
            "outerDiameter":       17.1 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         7/16 * inch,
            "outerDiameter":       16.83 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         1/2 * inch,
            "outerDiameter":       18.23 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         9/16 * inch,
            "outerDiameter":       19.72 * millimeter
        })));
        defaultSocket[].set_length(29.24 * millimeter);
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         5/8 * inch,
            "outerDiameter":       22.04 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         11/16 * inch,
            "outerDiameter":       24.29 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         3/4 * inch,
            "outerDiameter":       25.9 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         13/16 * inch,
            "outerDiameter":       27.97 * millimeter
        })));
        predefinedBitHolders[nameOfThisBitHolder] = socketHolder(sockets);
    }
    nameOfThisBitHolder = "1/4-inch drive, metric sockets holder"; {   
        var sockets =[];
        var defaultSocket = new_socket({
            "driveSize":           1/4 * inch,
            "length":              24.9 * millimeter,
            "nominalUnits":        "millimeter"
        });
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         4 * millimeter,
            "outerDiameter":       12 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         5 * millimeter,
            "outerDiameter":       12 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         6 * millimeter,
            "outerDiameter":       12 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         7 * millimeter,
            "outerDiameter":       12 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         8 * millimeter,
            "outerDiameter":       12 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         9 * millimeter,
            "outerDiameter":       13.07 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         10 * millimeter,
            "outerDiameter":       14.57 * millimeter
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         11 * millimeter,
            "outerDiameter":       16.01 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         12 * millimeter,
            "outerDiameter":       16.78 * millimeter,
        })));  
        predefinedBitHolders[nameOfThisBitHolder] = socketHolder(sockets);
    }
    nameOfThisBitHolder = "1/4-inch drive, imperial sockets holder"; {   
        var sockets =[];
        var defaultSocket = new_socket({
            "driveSize":           1/4 * inch,
            "length":              24.9 * millimeter,
            "nominalUnits":        "inch"
        });
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         5/32 * inch,
            "outerDiameter":       12 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         3/16 * inch,
            "outerDiameter":       12 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         7/32 * inch,
            "outerDiameter":       12 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         1/4 * inch,
            "outerDiameter":       12 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         9/32 * inch,
            "outerDiameter":       12 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         5/16 * inch,
            "outerDiameter":       12 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         11/32 * inch,
            "outerDiameter":       13.13 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         3/8 * inch,
            "outerDiameter":       14.60 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         7/16 * inch,
            "outerDiameter":       16.17 * millimeter,
        })));
        sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
            "nominalSize":         1/2 * inch,
            "outerDiameter":       17.78 * millimeter,
        })));
        predefinedBitHolders[nameOfThisBitHolder] = socketHolder(sockets);
    }
    

    
    //and now, a family of 1/4-inch hex shank bit holders:
    
    var dummyBitLength = 123.456*millimeter;
    // the designs below use a fixed bore depth, so the length of the bit is irrelevant, but needs to be something.
    
    //var boreDepth = 23 * millimeter;
    var defaultBoreDepth = 11 * millimeter;
    var deepBoreDepth = 23 * millimeter;
    
    var defaultBit = new_bit({ 
        "length":              dummyBitLength,
        "outerDiameter":       (1/4) * inch * 1/cos(30*degree), //circumcscribed circle diameter of regular hexagon having inscribed circle diameter 1/4 inch.
        "preferredLabelText":  "\\floodWithInk"
    });
    
    var defaultBitHolderSegment = 
        new_bitHolderSegment({
            "boreDiameterAllowance"            :  0.4*millimeter,
            "lecternAngle"                     :  60*degree,
            "labelFontHeight"                  :  [2.8*millimeter, 2.8*millimeter],
            "labelThickness"                   :  0.9*millimeter,
            "minimumAllowedLabelToZMinOffset"  :  2 * millimeter,
            "minimumAllowedBoreToZMinOffset"   :  2 * millimeter,
            "labelZMax"                        :  -3 * millimeter,
            "explicitLabelExtentX"             :  9.1 * millimeter,
            "labelExtentZ"                     :  12.1 * millimeter,
            "labelSculptingStrategy"           :  labelSculptingStrategy.ENGRAVE,
            "keepInkSheets"                    : false,
            "doLabelRetentionLip"              : true
    });
    
    var numberOfSegments = 20;
    
    var defaultBitHolder = new_bitHolder(
        {
           "mountHolesPositionZSpecifier":-10 * millimeter,
           "segments":[new_bitHolderSegment(defaultBitHolderSegment[])]
        }
    );
    
    
    nameOfThisBitHolder = "1/4-inch hex shank drill bits holder"; {   
        var boreDepth = deepBoreDepth;
        var bits = [];        
        {  //define bits
        
            
            //we use the socket class rather than bit in order (potentially) to take advantage of the socket class's "nominalSize" property t
            // ensure that we have enough spacece between adjacent bores in the case of especially large diameter dirll bits.
            // also, we might want to use the automatic label text generation facility built into the socket class.
            var defaultBit = new_socket(
                mergeMaps(
                    defaultBit[],
                    {
                        "driveSize":           999.123456 * inch,
                        "nominalUnits":        "inch",
                        "explicitLabelText":   "\\floodWithInk"
                    }
                )
            );
            
            bits = mapArray(
                [
                     4/64 * inch,
                     5/64 * inch,
                     6/64 * inch,
                     7/64 * inch,
                     8/64 * inch,
                     9/64 * inch,
                    10/64 * inch,
                    11/64 * inch,
                    12/64 * inch,
                    13/64 * inch,
                    14/64 * inch,
                    15/64 * inch,
                    16/64 * inch,
                    20/64 * inch,
                    24/64 * inch,
                    28/64 * inch,
                    32/64 * inch                        
                ],
                function(x){
                    return new_socket(
                        mergeMaps(
                            defaultBit[],
                            {
                                "nominalSize": x,
                                "preferredLabelText": "\\floodWithInk"
                            }
                        )
                    );
                }
            ); 
        }   
        
        var defaultBitHolderSegment = new_bitHolderSegment(
            mergeMaps(
                defaultBitHolderSegment[],
                {
                    "lecternAngle"  :  85*degree,
                }
            )
        );

        var bitHolder = new_bitHolder(defaultBitHolder[]);

        bitHolder[].set_segments(
            mapArray(
                bits, 
                function(theBit)
                {
                    var theBitHolderSegment = new_bitHolderSegment(defaultBitHolderSegment[]); 
                    theBitHolderSegment[].set_bit(theBit); 
                    theBitHolderSegment[].set_bitProtrusion(
                        theBitHolderSegment[].get_bit()[].get_length() - boreDepth
                    );
                    theBitHolderSegment[].set_angleOfElevation(theBitHolderSegment[].get_lecternAngle()); 
                    theBitHolderSegment[].set_minimumAllowedExtentX(max([theBitHolderSegment[].get_minimumAllowedExtentX(), theBit[].get_nominalSize()+1.1*millimeter ]));
                    return theBitHolderSegment;
                }
            )
        );
        predefinedBitHolders[nameOfThisBitHolder] = bitHolder;
    }
    nameOfThisBitHolder = "1/4-inch hex shank driver bits holder"; {   
        var boreDepth = defaultBoreDepth;
        var bitHolder = new_bitHolder(defaultBitHolder[]);
        bitHolder[].set_segments(
            mapArray(
                makeArray(numberOfSegments), 
                function(x)
                {
                    var theBitHolderSegment = new_bitHolderSegment(defaultBitHolderSegment[]); 
                    theBitHolderSegment[].set_bit(new_bit(defaultBit[])); 
                    theBitHolderSegment[].set_bitProtrusion(theBitHolderSegment[].get_bit()[].get_length() - boreDepth);
                    theBitHolderSegment[].set_angleOfElevation(theBitHolderSegment[].get_lecternAngle()); 
                    return theBitHolderSegment;
                }
            )  
        );
        predefinedBitHolders[nameOfThisBitHolder] = bitHolder;
    }
    nameOfThisBitHolder = "1/4-inch hex shank long driver bits holder"; {   
        var boreDepth = deepBoreDepth;
        var bitHolder = new_bitHolder(defaultBitHolder[]);
        bitHolder[].set_segments(
            mapArray(
                makeArray(numberOfSegments), 
                function(x)
                {
                    var theBitHolderSegment = new_bitHolderSegment(defaultBitHolderSegment[]); 
                    theBitHolderSegment[].set_bit(new_bit(defaultBit[])); 
                    theBitHolderSegment[].set_bitProtrusion(theBitHolderSegment[].get_bit()[].get_length() - boreDepth);
                    theBitHolderSegment[].set_angleOfElevation(theBitHolderSegment[].get_lecternAngle()); 
                    return theBitHolderSegment;
                }
            )  
        );
        predefinedBitHolders[nameOfThisBitHolder] = bitHolder;
    }
    
    
    var cannedDesigns = {};
    for(var entry in predefinedBitHolders){
        cannedDesigns[entry.key] = {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : function(context is Context, id is Id, definition is map){
                entry.value[].build(context, uniqueId(context, id));
            }
        };
    }
    
    if(false){
        cannedDesigns["bondhus_hex_drivers_holder"] = {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : function(context is Context, id is Id, definition is map){
                predefinedBitHolders["bondhus_hex_drivers_holder"][].build(context, uniqueId(context, id));
            }
        };
        cannedDesigns["3/8-inch drive, metric sockets holder"] =  {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : socketHolderFeature(
                (function(){            
                    var sockets =[];
                    var defaultSocket = new_socket({
                        "driveSize":           3/8 * inch,
                        "length":              25.96 * millimeter,
                        "nominalUnits":        "millimeter"
                    });
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         10 * millimeter,
                        "outerDiameter":       17.18 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         11 * millimeter,
                        "outerDiameter":       17.18 * millimeter 
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         12 * millimeter,
                        "outerDiameter":       17.8 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         13 * millimeter,
                        "outerDiameter":       18.3 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         14 * millimeter,
                        "outerDiameter":       19.71 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         15 * millimeter,
                        "outerDiameter":       20.43 * millimeter
                    })));
                    defaultSocket[].set_length(29.24 * millimeter);
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         17 * millimeter,
                        "outerDiameter":       23.36 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         19 * millimeter,
                        "outerDiameter":       25.87 * millimeter
                    })));
                    return sockets;
                })()
            )
        };
        cannedDesigns["3/8-inch drive, imperial sockets holder"] =  {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : socketHolderFeature(
                function(){            
                    var sockets =[];
                    var defaultSocket = new_socket({
                        "driveSize":           3/8 * inch,
                        "length":              25.96 * millimeter,
                        "nominalUnits":        "inch"
                    });
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         3/8 * inch,
                        "outerDiameter":       17.1 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         7/16 * inch,
                        "outerDiameter":       16.83 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         1/2 * inch,
                        "outerDiameter":       18.23 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         9/16 * inch,
                        "outerDiameter":       19.72 * millimeter
                    })));
                    defaultSocket[].set_length(29.24 * millimeter);
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         5/8 * inch,
                        "outerDiameter":       22.04 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         11/16 * inch,
                        "outerDiameter":       24.29 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         3/4 * inch,
                        "outerDiameter":       25.9 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         13/16 * inch,
                        "outerDiameter":       27.97 * millimeter
                    })));
                    return sockets;
                }()
            )
        };
        cannedDesigns["1/4-inch drive, metric sockets holder"] =  {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : socketHolderFeature(
                function(){            
                    var sockets =[];
                    var defaultSocket = new_socket({
                        "driveSize":           1/4 * inch,
                        "length":              24.9 * millimeter,
                        "nominalUnits":        "millimeter"
                    });
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         4 * millimeter,
                        "outerDiameter":       12 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         5 * millimeter,
                        "outerDiameter":       12 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         6 * millimeter,
                        "outerDiameter":       12 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         7 * millimeter,
                        "outerDiameter":       12 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         8 * millimeter,
                        "outerDiameter":       12 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         9 * millimeter,
                        "outerDiameter":       13.07 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         10 * millimeter,
                        "outerDiameter":       14.57 * millimeter
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         11 * millimeter,
                        "outerDiameter":       16.01 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         12 * millimeter,
                        "outerDiameter":       16.78 * millimeter,
                    })));  
                    return sockets;
                }()
            )
        };
        cannedDesigns["1/4-inch drive, imperial sockets holder"] =  {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : socketHolderFeature(
                function(){            
                    var sockets =[];
                    var defaultSocket = new_socket({
                        "driveSize":           1/4 * inch,
                        "length":              24.9 * millimeter,
                        "nominalUnits":        "inch"
                    });
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         5/32 * inch,
                        "outerDiameter":       12 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         3/16 * inch,
                        "outerDiameter":       12 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         7/32 * inch,
                        "outerDiameter":       12 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         1/4 * inch,
                        "outerDiameter":       12 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         9/32 * inch,
                        "outerDiameter":       12 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         5/16 * inch,
                        "outerDiameter":       12 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         11/32 * inch,
                        "outerDiameter":       13.13 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         3/8 * inch,
                        "outerDiameter":       14.60 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         7/16 * inch,
                        "outerDiameter":       16.17 * millimeter,
                    })));
                    sockets = append(sockets, new_socket(mergeMaps(defaultSocket[],{
                        "nominalSize":         1/2 * inch,
                        "outerDiameter":       17.78 * millimeter,
                    })));
                    return sockets;
                }()
            )
        };
        cannedDesigns["1/4-inch hex shank drill bits holder"] =  {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : function(context is Context, id is Id, definition is map){
                var boreDepth = deepBoreDepth;
                var bits = [];        
                {  //define bits
                
                    
                    //we use the socket class rather than bit in order (potentially) to take advantage of the socket class's "nominalSize" property t
                    // ensure that we have enough spacece between adjacent bores in the case of especially large diameter dirll bits.
                    // also, we might want to use the automatic label text generation facility built into the socket class.
                    var defaultBit = new_socket(
                        mergeMaps(
                            defaultBit[],
                            {
                                "driveSize":           999.123456 * inch,
                                "nominalUnits":        "inch",
                                "explicitLabelText":   "\\floodWithInk"
                            }
                        )
                    );
                    
                    bits = mapArray(
                        [
                             4/64 * inch,
                             5/64 * inch,
                             6/64 * inch,
                             7/64 * inch,
                             8/64 * inch,
                             9/64 * inch,
                            10/64 * inch,
                            11/64 * inch,
                            12/64 * inch,
                            13/64 * inch,
                            14/64 * inch,
                            15/64 * inch,
                            16/64 * inch,
                            20/64 * inch,
                            24/64 * inch,
                            28/64 * inch,
                            32/64 * inch                        
                        ],
                        function(x){
                            return new_socket(
                                mergeMaps(
                                    defaultBit[],
                                    {
                                        "nominalSize": x,
                                        "preferredLabelText": "\\floodWithInk"
                                    }
                                )
                            );
                        }
                    ); 
                }   
                
                var defaultBitHolderSegment = new_bitHolderSegment(
                    mergeMaps(
                        defaultBitHolderSegment[],
                        {
                            "lecternAngle"  :  85*degree,
                        }
                    )
                );
    
                var bitHolder = new_bitHolder(defaultBitHolder[]);
    
                bitHolder[].set_segments(
                    mapArray(
                        bits, 
                        function(theBit)
                        {
                            var theBitHolderSegment = new_bitHolderSegment(defaultBitHolderSegment[]); 
                            theBitHolderSegment[].set_bit(theBit); 
                            theBitHolderSegment[].set_bitProtrusion(
                                theBitHolderSegment[].get_bit()[].get_length() - boreDepth
                            );
                            theBitHolderSegment[].set_angleOfElevation(theBitHolderSegment[].get_lecternAngle()); 
                            theBitHolderSegment[].set_minimumAllowedExtentX(max([theBitHolderSegment[].get_minimumAllowedExtentX(), theBit[].get_nominalSize()+1.1*millimeter ]));
                            return theBitHolderSegment;
                        }
                    )
                );
                
                var bitHolderBody = bitHolder[].build(context, uniqueId(context, id));
            }
        };
        cannedDesigns["1/4-inch hex shank driver bits holder"] =  {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : function(context is Context, id is Id, definition is map){
                var boreDepth = defaultBoreDepth;
                var bitHolder = new_bitHolder(defaultBitHolder[]);
                bitHolder[].set_segments(
                    mapArray(
                        makeArray(numberOfSegments), 
                        function(x)
                        {
                            var theBitHolderSegment = new_bitHolderSegment(defaultBitHolderSegment[]); 
                            theBitHolderSegment[].set_bit(new_bit(defaultBit[])); 
                            theBitHolderSegment[].set_bitProtrusion(theBitHolderSegment[].get_bit()[].get_length() - boreDepth);
                            theBitHolderSegment[].set_angleOfElevation(theBitHolderSegment[].get_lecternAngle()); 
                            return theBitHolderSegment;
                        }
                    )  
                );
                var bitHolderBody = bitHolder[].build(context, uniqueId(context, id));
            }
        }; 
        cannedDesigns["1/4-inch hex shank long driver bits holder"] =  {
            "description" : "lorem ipsum dolorem",
            "featureFunction" : function(context is Context, id is Id, definition is map){
                var boreDepth = deepBoreDepth;
                var bitHolder = new_bitHolder(defaultBitHolder[]);
                bitHolder[].set_segments(
                    mapArray(
                        makeArray(numberOfSegments), 
                        function(x)
                        {
                            var theBitHolderSegment = new_bitHolderSegment(defaultBitHolderSegment[]); 
                            theBitHolderSegment[].set_bit(new_bit(defaultBit[])); 
                            theBitHolderSegment[].set_bitProtrusion(theBitHolderSegment[].get_bit()[].get_length() - boreDepth);
                            theBitHolderSegment[].set_angleOfElevation(theBitHolderSegment[].get_lecternAngle()); 
                            return theBitHolderSegment;
                        }
                    )  
                );
                var bitHolderBody = bitHolder[].build(context, uniqueId(context, id));
            }
        }; 
    }
    // COLLECTIONS OF bit-holders
    
    var quarterInchHexShankHolderKeys = [
        "1/4-inch hex shank drill bits holder", 
        "1/4-inch hex shank driver bits holder", 
        "1/4-inch hex shank long driver bits holder"
    ];
    
    var allKeys = keys(cannedDesigns);
    
    
    cannedDesigns["all bit holders"] = {
        "description" : "lorem ipsum dolorem",
        "featureFunction" : function(context is Context, id is Id, definition is map){
            var insertionPoint = WORLD_ORIGIN;
            var insertionPointIncrement = -2*inch*Z_DIRECTION;
            for(var entry in predefinedBitHolders){
                var idOfThisBitHolder = uniqueId(context, id);
                entry.value[].build(context, idOfThisBitHolder);
                var thisBitHolderBody = qCreatedBy(idOfThisBitHolder, EntityType.BODY);
                println("entry.value[].get_yMax(): " ~ entry.value[].get_yMax() );
                opTransform(context, uniqueId(context, id),
                    {
                        "bodies": thisBitHolderBody,
                        "transform": transform(
                            insertionPoint 
                            - (
                                entry.value[].get_mountHolePositions()[0] + entry.value[].get_yMax() * Y_DIRECTION
                            )
                        )
                    }
                );
                insertionPoint += insertionPointIncrement;
            }
        }
    }; 
    
    cannedDesigns["all 1/4-inch hex shank bit holders"] = {
        "description" : "lorem ipsum dolorem",
        "featureFunction" : function(context is Context, id is Id, definition is map){
            var insertionPoint = WORLD_ORIGIN;
            var insertionPointIncrement = 2*inch*Z_DIRECTION;
            for(var key in quarterInchHexShankHolderKeys){
                var idOfThisBitHolder = uniqueId(context, id);
                cannedDesigns[key].featureFunction(context, idOfThisBitHolder, {});
                var thisBitHolderBody = qCreatedBy(idOfThisBitHolder, EntityType.BODY);
                opTransform(context, uniqueId(context, id),
                    {
                        "bodies": thisBitHolderBody,
                        "transform": transform(insertionPoint)
                    }
                );
                insertionPoint += insertionPointIncrement;
            }
        }
    }; 
    
    //the below is a throw-away, just for testing
    cannedDesigns["defaultBitHolder"] =  {
        "description" : "lorem ipsum dolorem",
        "featureFunction" : function(context is Context, id is Id, definition is map){
            var defaultBitHolderSegment = new_bitHolderSegment();
            var bitHolder = 
                new_bitHolder(
                    {
                        "segments":
                            [
                                new_bitHolderSegment(mergeMaps(defaultBitHolderSegment[], {"bit": new_bit()})),
                                //new_bitHolderSegment(mergeMaps(defaultBitHolderSegment[], {"bit": new_bit()})),
                                //new_bitHolderSegment(mergeMaps(defaultBitHolderSegment[], {"bit": new_bit()}))
                            ]
                    }
                ); 
            
            //bitHolder[].get_segments()[0][].get_bit()[].set_preferredLabelText ( "lorem \\ahoy{}foo \\foo{}abc \\fullBox \\bar" );
            bitHolder[].get_segments()[0][].get_bit()[].set_preferredLabelText ( "lorem \\floodWithInk" );
            
            bitHolder[].get_segments()[0][].set_labelFontHeight(0.3 * millimeter);
            bitHolder[].get_segments()[0][].set_minimumAllowedLabelToZMinOffset(2 * millimeter);  
            bitHolder[].get_segments()[0][].set_minimumAllowedBoreToZMinOffset(2 * millimeter);  
            bitHolder[].get_segments()[0][].set_labelZMax(- 1 * millimeter);
            bitHolder[].get_segments()[0][].set_labelExtentX(10 * millimeter);
            bitHolder[].get_segments()[0][].set_labelExtentZ(12*millimeter);
            bitHolder[].get_segments()[0][].set_labelSculptingStrategy(labelSculptingStrategy.ENGRAVE); 
            bitHolder[].get_segments()[0][].set_labelThickness(0.9 * millimeter);
            
            var bitHolderBody = bitHolder[].build(context, uniqueId(context, id));
        }
    };

    
    return cannedDesigns;
}


//takes an array of sockets.  creates and returns a bitHolder to match.
function socketHolder(sockets) returns bitHolder
{
    return new_bitHolder(
        {"segments": 
            mapArray(
                sockets, 
                function(theSocket)
                {
                    var theBitHolderSegment = new_bitHolderSegment(); 
                    theBitHolderSegment[].set_bit(theSocket); 
                    theBitHolderSegment[].set_labelFontHeight(4.75 * millimeter);
                    return theBitHolderSegment;
                }
            )
        }
    );
}

//takes an array of sockets.  spits out the feature function for the socket holder
function socketHolderFeature(sockets) returns function
{
    return function(context is Context, id is Id, definition is map)
    {
        socketHolder(sockets)[].build(context, uniqueId(context, id));
    };
}

