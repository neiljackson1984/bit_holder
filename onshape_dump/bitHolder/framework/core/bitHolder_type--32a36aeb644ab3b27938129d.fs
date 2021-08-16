//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "20be3e9e3b832f6babff03e7", version : "fe25cd47b70ccda1fd60e890");
import(path : "e74f2213a201f3810593935d", version : "e7bdd71357900dc72524add5");
import(path : "ec66a324d53ae9ba8ca8980b", version : "bf09db9513dbe051f939a5cd");
import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");
import(path : "fa9afe329afdd32f7978375a", version : "3a81e66e5288275dacedcdfb");
import(path : "7718ed3c3facd33626142ef1", version : "51ca12e94c2f5e082eadb4cd");



export type bitHolderSegment typecheck isBitHolderSegment;

export predicate isBitHolderSegment(value)
{
       value is box;
       value[].bitHolder is bitHolder || value[].bitHolder is undefined;
}

export enum labelSculptingStrategy 
{
    EMBOSS,
    ENGRAVE
}

//constructor for bitHolderSegment
export function new_bitHolderSegment() returns bitHolderSegment
{
    
    var this = new box({}) as bitHolderSegment;
    this[].bit = new_bit();
    // this[].bitHolder = new_bitHolder({segments:[this]});  //this does not work, because bitHolder::set_segments calls the set_bitHolder method of each segment in segments, and, at this point, our nascent bitHolderSegment does not yet have a set_bitHolder() method. 
    this[].bitHolder = new_bitHolder();//new box({segments:[this as bitHolderSegment]}) as bitHolder;//new_bitHolder({segments: [this]});
    this[].angleOfElevation = 45 * degree;
    this[].lecternAngle = 45 * degree;
    this[].lecternMarginBelowBore = 3 * millimeter;
    this[].lecternMarginAboveBore = 3 * millimeter;
    this[].boreDiameterAllowance = 0.8 * millimeter;
    
    this[].mouthFilletRadius = 2*millimeter;
    
    //this[].bitProtrusionY = 10 * millimeter;
    this[].bitProtrusion = 10.6 * millimeter;
    this[].keepInkSheets = false; //not yet implemented //a flag to control whether we keep the label's ink sheet bodies after we are finished with them.  This can be useful for visualizing text.
    this[].labelExtentZ= 12 * millimeter;
    this[].labelThickness= 0.9 * millimeter; //this is the thickness of the text (regardless of whether the text is engraved or embossed).
    this[].labelZMax = - 1 * millimeter;
    this[].labelFontHeight = 4.75 * millimeter;
    this[].labelFontName = "NotoSans-Bold.ttf";
    this[].labelSculptingStrategy = labelSculptingStrategy.ENGRAVE; //labelSculptingStrategy.EMBOSS;
    this[].minimumAllowedExtentX = 12 * millimeter; //used to enforce miimum interval between bits for ease of grabbing very smalle bits with fingers.
    this[].minimumAllowedExtentY = 0 * millimeter; 
    this[].minimumAllowedLabelToZMinOffset = 0 * millimeter;  
    this[].minimumAllowedBoreToZMinOffset = 2 * millimeter;  
    this[].explicitLabelExtentX  = false;
    this[].doLabelRetentionLip = false; //This only makes sense in the case where we are using the "floodWithInk" label text mechanism to generate a rectangular pocket (intended to hold a printed paper card).  
    // In that case, we can sweep a "retention lip" profile around (some of) the edges of the pocket that the edges of the paper card can be tucked under to hold the card in place.
    
    this[].labelRetentionLipProfile = [
        vector(zeroLength, zeroLength),
        vector(0.2*millimeter, zeroLength),
        vector(zeroLength, -0.07*millimeter)
        
    ];
    
    this[].directionsOfEdgesThatWeWillAddALabelRetentionLipTo = [X_DIRECTION];
    
    //we will not add a lip to all edges.  Rather, we will add a lip only to edges that are parallel (possibly within some tolerance) 
    // (or anti-parallel)
    // to any of the specified directions.  (this is geared toward the cases where edges are straight lines.)
    
    
    //test case:
    // this[].labelRetentionLipProfile = [
    //     vector(zeroLength, 0.1*millimeter),
    //     vector(0.2*millimeter, 0.1*millimeter),
    //     vector(0.2*millimeter, 0.3*millimeter),
    //     vector(0.4*millimeter, 0.3*millimeter),
    //     vector(0.2*millimeter, zeroLength),
    //     vector(zeroLength, -0.07*millimeter)
    // ];
    var a = 0.4*millimeter;
    var b = 0.4*millimeter;
    var c = 0.03*millimeter;
    this[].labelRetentionLipProfile = [
        vector(zeroLength, zeroLength),
        vector(a, zeroLength),
        vector(a, -b + c),
        vector(zeroLength, -b)
    ];

    // This is a list of 2dPointVector dexcribing the polygonal cross-section of the labelRetentionLip.  The positive Y direction points out of the pocket.  The origin is on the upper edge of the side-wall of the pocket.  
    // the X direction is perpendicular to that edge.  positive X points "off the edge of the cliff".
    
    this[].get_zMax = function(){
        //   return
        //         max(
        //             10 * millimeter,
        //             //this[].get_boreDiameter()/cos(this[].get_angleOfElevation()) + this[].mouthFilletRadius + 1.1 * millimeter
        //             dot(this[].get_lecternTopPoint(), zHat)
        //         );
            return dot(this[].get_lecternTopPoint(), zHat);
        };
    
    this[].get_boreDepth = 
        function()
        {
            //return 14 * millimeter; 
            return this[].get_bit()[].length - this[].get_bitProtrusion();
        };
        
    this[].get_zMin = 
        function()
        {
            return min(
                dot(
                    zHat, 
                    this[].get_boreBottomCenter() + this[].get_boreDiameter()/2 * (rotationMatrix3d(xHat, 90 * degree) * this[].get_boreDirection())
                ) - this[].get_minimumAllowedBoreToZMinOffset(),
                this[].get_labelZMin() - this[].get_minimumAllowedLabelToZMinOffset()
            );
        };
    this[].get_extentX = function()
        {
            return 
                max([
                    this[].get_minimumAllowedExtentX(),
                    this[].get_boreDiameter() + 2*this[].get_mouthFilletRadius() + 0.2 * millimeter,
                    this[].get_boreDiameter() + 2 * millimeter
                ]);
        };
        
    this[].get_extentY = function()
        {
            return 
                max(
                    [
                        this[].get_minimumAllowedExtentY(),
                        //the minimum thickness to guarantee that the bore does not impinge on the mount hole or the clearance zone for the head of the mount screw.
                        this[].get_bitHolder()[].get_mountHole()[].get_minimumAllowedClampingThickness()
                        + this[].get_bitHolder()[].get_mountHole()[].get_headClearanceHeight()
                        + dot(
                            yHat,
                            this[].get_boreBottomCenter() + this[].get_boreDiameter()/2 * (rotationMatrix3d(xHat, -90 * degree) * this[].get_boreDirection())
                        )
                    ]
                );
        };
        
        
    this[].get_labelExtentX = function(){
            if(this[].get_explicitLabelExtentX() is ValueWithUnits)
            {
                return this[].get_explicitLabelExtentX();
            } else
            {
                
                return this[].get_extentX() - 0.4 * millimeter;
            }
        };
            
    this[].set_labelExtentX = function(x){
            this[].explicitLabelExtentX = x;   
        };
 
    
    this[].get_labelText = function(){
        return this[].get_bit()[].get_preferredLabelText(); // "\u0298" is a unicode character that, at least in the Tinos font, consists of a circle, like an 'O', and a central isolated dot.  This character does not extrude correctly (the middle dot is missng)
        };
    this[].get_labelZMin = function(){
        return this[].get_labelZMax()  - this[].get_labelExtentZ();};
    this[].get_labelXMin = function(){
        return -this[].get_labelExtentX()/2;  };
    this[].get_labelXMax = function(){
        return  this[].get_labelExtentX()/2;  };
    this[].get_labelExtentY = function(){
        return this[].get_labelThickness(); };
    this[].get_labelYMin = function(){
        return (this[].get_labelSculptingStrategy()==labelSculptingStrategy.ENGRAVE ? 0 : -1) * this[].get_labelExtentY() ;};
    this[].get_labelYMax = function(){
        return this[].get_labelYMin() + this[].get_labelExtentY();};
    
    
    
    
    
    this[].yMin = 0 * meter;
    this[].get_yMax = function(){
        return this[].get_extentY();};
    
    this[].get_xMin = function(){
        return -this[].get_extentX()/2;};
    this[].get_xMax = function(){
        return this[].get_extentX()/2;};
    this[].get_origin = function(){return vector(0,0,0) * zeroLength;};
    
    this[].get_boreDirection = 
        function()
        {
            return rotationMatrix3d(xHat, -this[].get_angleOfElevation()) * -yHat; //shooting out of the bore
        };
    
    this[].get_boreDiameter = 
        function()
        {
            return 
                this[].get_bit()[].get_outerDiameter() 
                + this[].get_boreDiameterAllowance();
        };
    

        
    this[].get_boreTopCenter = 
        function()
        {
            //return 
                // vector(0, -1, tan(this[].get_angleOfElevation()))*this[].get_bitProtrusionY()
                // + this[].get_boreDiameter()/2 * (rotationMatrix3d(xHat,-90*degree)*this[].get_boreDirection());
            return this[].get_boreBottomCenter() + 1 * meter * this[].get_boreDirection();
        };
    
    this[].get_lecternNormal = 
        function()
        {
            return rotationMatrix3d(xHat, -this[].get_lecternAngle()) * -yHat;  
        };
        
    this[].get_boreBottomCenter = 
        function()
        {
            // return this[].get_boreTopCenter() - this[].get_boreDirection() * this[].get_bit()[].get_length();
            return this[].get_borePiercePoint() - this[].get_boreDepth() * this[].get_boreDirection();
        };
    
    this[].get_bottomBoreCorner = 
        function()
        {
            return 
                this[].get_origin() -(rotationMatrix3d(xHat, 90*degree) * this[].get_lecternNormal())
                * (
                    this[].get_lecternMarginBelowBore()
                );
        };
    
    this[].get_borePiercePoint = 
        function()
        {
            // return
            //     this[].get_origin() + 
            //     vector(0, sin(this[].get_lecternAngle()), cos(this[].get_lecternAngle())) 
            //     * (
            //         this[].get_lecternMarginBelowBore() 
            //         + (1/2) * 1/sin(90*degree - this[].get_angleOfElevation() + this[].get_lecternAngle()) * this[].get_boreDiameter()
            //     );
                
            return 
                this[].get_origin() - 
                (rotationMatrix3d(xHat, 90*degree) * this[].get_lecternNormal())
                * (
                    this[].get_lecternMarginBelowBore()
                    + (1/2) * 1/sin(90*degree - this[].get_angleOfElevation() + this[].get_lecternAngle()) * this[].get_boreDiameter()
                );
        };
        
    this[].get_topBoreCorner = 
        function()
        {
            return 
                this[].get_origin() - 
                (rotationMatrix3d(xHat, 90*degree) * this[].get_lecternNormal())
                * (
                    this[].get_lecternMarginBelowBore()
                    + 1/sin(90*degree - this[].get_angleOfElevation() + this[].get_lecternAngle()) * this[].get_boreDiameter()
                );
        };
        
    this[].get_lecternTopPoint = 
        function()
        {
            return 
                this[].get_origin() - 
                (rotationMatrix3d(xHat, 90*degree) * this[].get_lecternNormal())
                * (
                    this[].get_lecternMarginBelowBore()
                    + 1/sin(90*degree - this[].get_angleOfElevation() + this[].get_lecternAngle()) * this[].get_boreDiameter()
                    + this[].get_lecternMarginAboveBore()
                );
        };
    
    this[].get_bottomPointOfMouthFilletSweepPath = 
        function()
        {
            return 
                this[].get_bottomBoreCorner() + 
                1/sin((90*degree - this[].get_angleOfElevation() + this[].get_lecternAngle() )/2) * this[].get_mouthFilletRadius()
                * normalize(
                    mean([
                        rotationMatrix3d(xHat, 90*degree) * this[].get_lecternNormal(),
                        -this[].get_boreDirection()
                    ])
                );
        };
    
    
    // this[].get_topPointOfMouthFilletSweepPath = 
    //     function()
    //     {
    //         return 
    //             this[].get_bottomBoreCorner() + 
    //             1/sin((90*degree - this[].get_angleOfElevation() + this[].get_lecternAngle() )/2) * this[].get_mouthFilletRadius()
    //             * normalize(
    //                 mean([
    //                     rotationMatrix3d(xHat, 90*degree) * this[].get_lecternNormal(),
    //                     -this[].get_boreDirection()
    //                 ])
    //             );
    //     };
        
        
    
    this[].get_bottomSaddlePointOfMouthFillet = //see neil-4936
        function()
        {                
            return this[].get_bottomPointOfMouthFilletSweepPath() + this[].get_mouthFilletRadius() * zHat;
        };
    
    //I am still not totally satisfied with this method of simulating a member function like this -- I don't like the fact that the member function is a lambda that could inadvertently be overwritten.
    this[].build = 
        function(context is Context, id is Id)
        {
            var idOfInitialBodyCreationOperation = id + "mainBody";
            // fCuboid(
            //     context,
            //     idOfInitialBodyCreationOperation,
            //     {
            //         corner1:vector(this[].get_xMin(),this[].get_yMin(),this[].get_zMin()),
            //         corner2:vector(this[].get_xMax(),this[].get_yMax(),this[].get_zMax())
            //     }
            // );
            
            var polygonVertices = 
                [
                    vector(this[].get_yMin(), zeroLength),
                    vector(this[].get_yMin(), zeroLength) + vector(tan(this[].get_lecternAngle()) * this[].get_zMax() , this[].get_zMax()),
                    vector(this[].get_yMax(),this[].get_zMax()),
                    vector(this[].get_yMax(),this[].get_zMin()),
                    vector(this[].get_yMin(),this[].get_zMin()),
                ];
            createRightPolygonalPrism(
                context, 
                idOfInitialBodyCreationOperation, 
                {
                    "plane": plane(vector(zeroLength,zeroLength,zeroLength), xHat, yHat),
                    "vertices":
                        polygonVertices,
                    "height":this[].get_extentX()
                }
            );
            
            var mainBody = qBodyType(qCreatedBy(idOfInitialBodyCreationOperation, EntityType.BODY), BodyType.SOLID);
            var returnValue = mainBody;
            //we now have the mainBody, which we will proceed to modify below.
            // As a side-effect of our modifications, we may end up with leftover bodies that were used for construction
            // We want to be sure to delete any of these leftover bodies before we return from this build() function.
            // we will collect throwaway entities that need to be deleted in throwAwayEntities:
            // var throwAwayEntities = qNothing();
            //Actually, I suspect that we might be able to accomplish this goal
            // by doing an opDelete on a query that finds all bodies "created by" id, except mainBody.
            // This will work assuming that qCreatedBy(id, EntityType.body), returns all bodies
            // that were created by any operation whose id is descended from id, because all
            // the operation ids that I construct in this build() function I construct
            // using uniquid(context, id), and the uniqueid function returns an id that is descended from the input id.
            
            //println("reportOnEntities(context, mainBody): " ~ reportOnEntities(context, mainBody, 0, 0));
            
            // mapArray(
            //     polygonVertices,
            //     function(vertex)
            //     {
            //         var vertex3d = vector(zeroLength, vertex[0], vertex[1]);  
            //         debug(context, vertex3d);
            //     }
            // );
            
            var idOfBore = id + "bore";
            var idOfBoreTool = id + "boretool";
            // debug(context, qCreatedBy(idOfInitialBodyCreationOperation));
            // debug(context, this[].get_borePiercePoint());
            // println("this[].get_boreBottomCenter(): " ~ toString(this[].get_boreBottomCenter()));		// this[].get_boreBottomCenter()
            // println("this[].get_boreTopCenter(): " ~ toString(this[].get_boreTopCenter()));		// this[].get_boreTopCenter()
            fCylinder(
                context,
                idOfBoreTool,
                {
                    topCenter: this[].get_boreTopCenter(),
                    bottomCenter: this[].get_boreBottomCenter(),
                    radius: this[].get_boreDiameter()/2
                }
            );
            //debug(context, qCreatedBy(idOfBoreTool, EntityType.BODY));
            
            
            var idOfSplitFace = uniqueId(context,id);
            opSplitFace(context,idOfSplitFace,
                {
                    // faceTargets: qCreatedBy(idOfInitialBodyCreationOperation, EntityType.FACE),
                    faceTargets: qOwnedByBody(mainBody, EntityType.FACE),
                    bodyTools: qCreatedBy(idOfBoreTool, EntityType.BODY)
                }
            );
            

            
            opBoolean(context, idOfBore,
                {
                    tools: qCreatedBy(idOfBoreTool, EntityType.BODY),
                    targets: mainBody,
                    operationType: BooleanOperationType.SUBTRACTION,
                    targetsAndToolsNeedGrouping:true
                }
            );
            //debug(context, qCreatedBy(idOfSplitFace, EntityType.EDGE));
            var edgesToFillet = qCreatedBy(idOfSplitFace, EntityType.EDGE);
            
            var idOfFillet =  uniqueId(context,id);
            try silent
            {
                opFillet(context, idOfFillet,
                    {
                        entities:edgesToFillet,
                        radius:this[].get_mouthFilletRadius(),
                        tangentPropagation:false
                    }
                );
            }
            
            var myGalley = new_galley();
            myGalley[].fontName = this[].get_labelFontName();
            myGalley[].rowSpacing = 1.3; 
            myGalley[].rowHeight = this[].get_labelFontHeight();
            myGalley[].text = this[].get_labelText();
            myGalley[].horizontalAlignment = horizontalAlignment.CENTER;
            myGalley[].verticalAlignment = verticalAlignment.TOP;
            myGalley[].clipping = true;
            myGalley[].width = this[].get_labelExtentX();
            myGalley[].height = this[].get_labelExtentZ();

            myGalley[].anchor = galleyAnchor_e.CENTER;
            myGalley[].worldPlane = 
                plane(
                    /* origin: */ vector(
                        mean([this[].get_labelXMin(), this[].get_labelXMax()]),
                        this[].get_labelYMin(), 
                        mean([this[].get_labelZMin(), this[].get_labelZMax()])
                    ),
                    /* normal: */ -yHat,
                    /* x direction: */ xHat  
                );
            var sheetBodiesInWorld = myGalley[].buildSheetBodiesInWorld(context, uniqueId(context,id));
            
            var idOfLabelTool = uniqueId(context, id);
            try
            {
                opExtrude(
                    context,
                    idOfLabelTool,
                    {
                        entities:  qOwnedByBody(sheetBodiesInWorld, EntityType.FACE),
                        direction: yHat,
                        endBound: BoundingType.BLIND,
                        endDepth: this[].get_labelThickness(),
                        startBound: BoundingType.BLIND,
                        startDepth: zeroLength
                    }
                );
            }
            
            // sculpt (i.e. either emboss or engrave, according to this[].label.sculptingStrategy) the label tool onto the main body.
            var idOfLabelSculpting = uniqueId(context,id);
            try {opBoolean(context, idOfLabelSculpting,
                {
                    tools: qCreatedBy(idOfLabelTool, EntityType.BODY),
                    targets: mainBody,
                    operationType:  (this[].get_labelSculptingStrategy()==labelSculptingStrategy.ENGRAVE ? BooleanOperationType.SUBTRACTION : BooleanOperationType.UNION),
                    targetsAndToolsNeedGrouping:true, //regardless of whether the tool was kissing the main body, disjoint from the main body, or overlapping the main body, the feature failed unless targetsAndToolsNeedGrouping was true.  when kssing, a single Part was created.  When disjoint, two Part(s) were created.
                    keepTools:false
                }
            );}
            
            if(this[].get_doLabelRetentionLip()){
                //the edges that we might want to sweep along are the set of edges e such that:
                // 1) e was created by the labelSculpting operation 
                // and 2) e bounds a face that existed before the label sculpting operation (i.e. a face that the labelSculpting operation modified but did not create.)
                // another way to state condition 2 is: e bounds a face that was not created by the label sculpting region.
                // another way to state condition 2 is: there exists a face not created by the label sculpting operation that owns e
                
                //var facesCreatedByTheLabelSculptingOperation = qCreatedBy(idOfLabelSculpting, EntityType.FACE);
                //println("facesCreatedByTheLabelSculptingOperation: ");print(reportOnEntities(context, facesCreatedByTheLabelSculptingOperation,0));
                //var sweepingEdgeCandidates = qCreatedBy(idOfLabelSculpting, EntityType.EDGE);
                var sweepingEdgeCandidates = qLoopEdges(qCreatedBy(idOfLabelSculpting, EntityType.FACE)); 
                //debug(context, sweepingEdgeCandidates);
                //debug(context, mainBody);
                
                
                // test case:
                // var sweepingEdges = qUnion(
                //     mapArray(
                //         connectedComponents(context, sweepingEdgeCandidates, AdjacencyType.VERTEX),
                //         function(x){return qUnion(array_slice(x,1));}
                //     )
                // );
                var sweepingEdges = qUnion(
                    filter(
                        evaluateQuery(context, sweepingEdgeCandidates)  ,
                        function(sweepingEdgeCandidate){
                            var edgeTangentDirection = evEdgeTangentLine(context, {edge: sweepingEdgeCandidate, parameter: 0}).direction;
                            
                            // we return true iff. there is at least one element of get_directionsOfEdgesThatWeWillAddALabelRetentionLipTo() 
                            // to which sweepingEdgeCandidate is parallel.
                            for(var direction in this[].get_directionsOfEdgesThatWeWillAddALabelRetentionLipTo()){
                                //evidently, the parallelVectors() function returns true in the case where the vectors parallel AND in
                                // the case where the vectors are anti-parallel.  That is an important fact that
                                // the documentation omits.
                                // fortunately, in our case, this is precisely the behavior that we want.
                                if(parallelVectors(edgeTangentDirection, direction)){
                                    // if(dot(edgeTangentDirection, direction) < 0){
                                    //     println(
                                    //         "that's interesting, the parallelVectors() function regards two vectors as parallel "
                                    //         ~ " even though the angle between them is " ~ 
                                    //         (acos(dot(normalize(edgeTangentDirection), normalize(direction)))/degree) //angleBetween(,direction) 
                                    //         ~ " degrees, "
                                    //         ~ "which is not zero."
                                    //     );
                                    //}
                                    
                                    return true;   
                                }
                            }
                            return false;
                        }
                    )
                );
                
                
                var lipBodies = qNothing();
                
                //we want to iterate over all connected chains of edges that exist within the set of sweepingEdges.
                //for each connected chain, we will sweep a lip.
                //var chainCandidates = connectedComponents(context, sweepingEdges, AdjacencyType.VERTEX);
                
                //println("size(chainCandidates): " ~ size(chainCandidates));
                
                
                // for(var chainCandidate in chainCandidates){
                //     println("size(chainCandidate): " ~ size(chainCandidate));
                //     for(var chainCandidateElement in chainCandidate){
                //         debug(context, chainCandidateElement) ;  
                //     }
                // }
                
                var chains = mapArray(
                    connectedComponents(context, sweepingEdges, AdjacencyType.VERTEX),
                    function(x){return qUnion(x);}
                );
                
                
                
                //println("size(chains): " ~ size(chains));
                //debug(context, chains[1]);
                //for(var chain in chains){
                //    debug(context, chain);   
                //}
                

                
                
                var weAreOnTheFirstIteration = true;
                if(false){
                    for (var sweepingEdge in evaluateQuery(context, sweepingEdgeCandidates)){
                        
                        var hostFace = qSubtraction(
                            qAdjacent(sweepingEdge, AdjacencyType.EDGE, EntityType.FACE),
                            qCreatedBy(idOfLabelSculpting)
                        );
                        
                        if(weAreOnTheFirstIteration){println("hostFace: ");print(reportOnEntities(context, hostFace,0,0));}
                        
                        // var yDirection = -yHat;
                        // In our case, we have constructed things above so that we know that the normal
                        // of the host facce is -yHat.  However, let's write this in a way that would work generally:
                        var yDirection = evFaceNormalAtEdge(context,
                            {
                                edge: sweepingEdge,
                                face: hostFace,
                                parameter: 0
                            }
                        );
                        
                        var edgeTangentLine = evEdgeTangentLine(context, {
                            edge: sweepingEdge, 
                            parameter: 0,
                            // face is expected to be a face adjacent to edge.  The direction of the returned
                            // tangent line will be such that when walking in that direction with the face 
                            // normal being "up", the face will be on the left.
                            // the face that we want to use (which might now no longer exist) is the face of the
                            // ink sheet (the face such that gazing ant-parallel to the face's normal at the text, we will 
                            // see the text in the "correct" chirality (i.e. not a mirror image))
                            //because that face probably doesn't exist any more (it was a temporary construction thing to create the extruded text),
                            //we will instead provide the OTHER face that owns sweeping edge, namely, the face that the extruded text was cut into (or 
                            // embossed out of (the face whose normal is yDirection)
                            face: hostFace
                        });
                        
                        var zDirection =  -edgeTangentLine.direction;
                        
                        
                        var labelRetentionLipCrossSectionSheetBody = createPolygonalSheetBody(context, uniqueId(context, id),
                            {
                                //we want a coordinate system whose y axis points "up" out of the pocket. 
                                // whose z axis is along (tangent or anti-tangent) to the edge, directed in such a way so 
                                // that the x axis points "off the edge of the cliff"
                                // and whose origin is on the edge
                                "coordSystem": 
                                    coordSystem(
                                        /* origin: */ 
                                        //I am assuming that the origin of the line returned by evEdgeTangentLine is the tangent point.
                                        //This is not guaranteed anywhere in the documentation, but ios probably a safe assumption.
                                        edgeTangentLine.origin,
                                         
                                        /* xAxis:  */  
                                        cross(yDirection, zDirection), 
                                        
                                        /* zAxis:  */  
                                        zDirection
                                    ),
                                "vertices": this[].get_labelRetentionLipProfile()
                            }                        
                        );
                        if(weAreOnTheFirstIteration){debug(context, labelRetentionLipCrossSectionSheetBody);}
                        //if(weAreOnTheFirstIteration){debug(context, qOwnedByBody(labelRetentionLipCrossSectionSheetBody, EntityType.EDGE));}
                        var faceToSweep = qOwnedByBody(labelRetentionLipCrossSectionSheetBody, EntityType.FACE);
                        if(weAreOnTheFirstIteration){println("faceToSweep: ");print(reportOnEntities(context, faceToSweep,0,0));}
                        
                        var idOfLipSweep = uniqueId(context, id);
                        
                        opSweep(context, idOfLipSweep,
                            {
                                profiles: faceToSweep,
                                path: sweepingEdge,
                                keepProfileOrientation: false,
                                lockFaces: qNothing()
                            }
                        );
                        
                        var lipBody = qCreatedBy(idOfLipSweep, EntityType.BODY);
                        lipBodies = qUnion([lipBodies, lipBody]);
                        //if(weAreOnTheFirstIteration){debug(context, lipBody);}
                        if(weAreOnTheFirstIteration){println("lipBody: ");print(reportOnEntities(context, lipBody,0,0));}
                        
                        
                        if(weAreOnTheFirstIteration){
                            var idOfOperationToCreateDisposableCopyOfMainBody = uniqueId(context, id);
                            opPattern(context, idOfOperationToCreateDisposableCopyOfMainBody,
                                {
                                    entities: mainBody,
                                    transforms: [identityTransform()],
                                    instanceNames: ["disposableCopy"],
                                    copyPropertiesAndAttributes: true
                                }
                            );
                            
                            var disposableCopyOfMainBody = qCreatedBy(idOfOperationToCreateDisposableCopyOfMainBody, EntityType.BODY);
                            var idOfOperationJoiningLipBodyToTheDisposableCopyOfTheMainBody = uniqueId(context, id);
                            opBoolean(context,idOfOperationJoiningLipBodyToTheDisposableCopyOfTheMainBody,
                                {
                                    operationType: BooleanOperationType.UNION,
                                    tools: qUnion([ disposableCopyOfMainBody, lipBody ]),
                                    //targets: qNothing(),
                                    keepTools:false,
                                    //targetsAndToolsNeedGrouping: false,
                                    //matches: [],
                                    //recomputeMatches: false
                                }
                            ); 
                            if(weAreOnTheFirstIteration){debug(context, disposableCopyOfMainBody);}
                            //if(weAreOnTheFirstIteration){debug(context, qOwnedByBody(disposableCopyOfMainBody, EntityType.EDGE));}
                        }
                        
                        
                        
                        weAreOnTheFirstIteration = false;
                    }  
                }
                
                
                for (var chain in chains){
                    
                    var sampleEdge = qNthElement(chain, 0);
                    var hostFace = qSubtraction(
                        qAdjacent(sampleEdge, AdjacencyType.EDGE, EntityType.FACE),
                        qCreatedBy(idOfLabelSculpting)
                    );
                    
                    //if(weAreOnTheFirstIteration){println("hostFace: ");print(reportOnEntities(context, hostFace,0,0));}
                    
                    // var yDirection = -yHat;
                    // In our case, we have constructed things above so that we know that the normal
                    // of the host facce is -yHat.  However, let's write this in a way that would work generally:
                    var yDirection = evFaceNormalAtEdge(context,
                        {
                            edge: sampleEdge,
                            face: hostFace,
                            parameter: 0
                        }
                    );
                    
                    var edgeTangentLine = evEdgeTangentLine(context, {
                        edge: sampleEdge, 
                        parameter: 0,
                        // face is expected to be a face adjacent to edge.  The direction of the returned
                        // tangent line will be such that when walking in that direction with the face 
                        // normal being "up", the face will be on the left.
                        // the face that we want to use (which might now no longer exist) is the face of the
                        // ink sheet (the face such that gazing ant-parallel to the face's normal at the text, we will 
                        // see the text in the "correct" chirality (i.e. not a mirror image))
                        //because that face probably doesn't exist any more (it was a temporary construction thing to create the extruded text),
                        //we will instead provide the OTHER face that owns sweeping edge, namely, the face that the extruded text was cut into (or 
                        // embossed out of (the face whose normal is yDirection)
                        face: hostFace
                    });
                    
                    var zDirection =  -edgeTangentLine.direction;
                    
                    
                    var labelRetentionLipCrossSectionSheetBody = createPolygonalSheetBody(context, uniqueId(context, id),
                        {
                            //we want a coordinate system whose y axis points "up" out of the pocket. 
                            // whose z axis is along (tangent or anti-tangent) to the edge, directed in such a way so 
                            // that the x axis points "off the edge of the cliff"
                            // and whose origin is on the edge
                            "coordSystem": 
                                coordSystem(
                                    /* origin: */ 
                                    //I am assuming that the origin of the line returned by evEdgeTangentLine is the tangent point.
                                    //This is not guaranteed anywhere in the documentation, but ios probably a safe assumption.
                                    edgeTangentLine.origin,
                                     
                                    /* xAxis:  */  
                                    cross(yDirection, zDirection), 
                                    
                                    /* zAxis:  */  
                                    zDirection
                                ),
                            "vertices": this[].get_labelRetentionLipProfile()
                        }                        
                    );
                    //if(weAreOnTheFirstIteration){debug(context, labelRetentionLipCrossSectionSheetBody);}
                    //if(weAreOnTheFirstIteration){debug(context, qOwnedByBody(labelRetentionLipCrossSectionSheetBody, EntityType.EDGE));}
                    var faceToSweep = qOwnedByBody(labelRetentionLipCrossSectionSheetBody, EntityType.FACE);
                    //if(weAreOnTheFirstIteration){println("faceToSweep: ");print(reportOnEntities(context, faceToSweep,0,0));}
                    
                    var idOfLipSweep = uniqueId(context, id);
                    
                    try{
                        opSweep(context, idOfLipSweep,
                            {
                                profiles: faceToSweep,
                                path: chain,
                                keepProfileOrientation: false,
                                lockFaces: qNothing()
                            }
                        );
                    } catch (error){
                        println("An exception occured during sweep: " ~ toString(error));
                        println(toString(getFeatureStatus(context, idOfLipSweep)));
                    }
                    
                   
                    
                    var lipBody = qCreatedBy(idOfLipSweep, EntityType.BODY);
                    if(queryReturnsSomething(context, lipBody)){
                        
                        lipBodies = qUnion([lipBodies, lipBody]);
                        //debug(context, qOwnedByBody(lipBody,EntityType.EDGE));
                        //if(weAreOnTheFirstIteration){println("lipBody: ");print(reportOnEntities(context, lipBody,0,0));}
                        
                        
                        if(false){
                            var idOfOperationToCreateDisposableCopyOfMainBody = uniqueId(context, id);
                            opPattern(context, idOfOperationToCreateDisposableCopyOfMainBody,
                                {
                                    entities: mainBody,
                                    transforms: [identityTransform()],
                                    instanceNames: ["disposableCopy"],
                                    copyPropertiesAndAttributes: true
                                }
                            );
                            var disposableCopyOfMainBody = qCreatedBy(idOfOperationToCreateDisposableCopyOfMainBody, EntityType.BODY);
                            
                            var idOfOperationToCreateDisposableCopyOfLipBody = uniqueId(context, id);
                            opPattern(context, idOfOperationToCreateDisposableCopyOfLipBody,
                                {
                                    entities: lipBody,
                                    transforms: [identityTransform()],
                                    instanceNames: ["disposableCopy"],
                                    copyPropertiesAndAttributes: true
                                }
                            );
                            var disposableCopyOfLipBody = qCreatedBy(idOfOperationToCreateDisposableCopyOfLipBody, EntityType.BODY);
                            
                            
                            
                            var idOfOperationJoiningTheDisposableCopyOfLipBodyToTheDisposableCopyOfMainBody = uniqueId(context, id);
                            opBoolean(context,idOfOperationJoiningTheDisposableCopyOfLipBodyToTheDisposableCopyOfMainBody,
                                {
                                    operationType: BooleanOperationType.UNION,
                                    tools: qUnion([ disposableCopyOfMainBody, disposableCopyOfLipBody ]),
                                    //targets: qNothing(),
                                    keepTools:false,
                                    //targetsAndToolsNeedGrouping: false,
                                    //matches: [],
                                    //recomputeMatches: false
                                }
                            ); 
                            if(weAreOnTheFirstIteration){debug(context, disposableCopyOfMainBody);}
                            //if(weAreOnTheFirstIteration){debug(context, qOwnedByBody(disposableCopyOfMainBody, EntityType.EDGE));}
                        }
                    }
                    
                    
                    
                    weAreOnTheFirstIteration = false;
                }
                
                
                //println("lipBodies: ");print(reportOnEntities(context, lipBodies,0,0));
                var idOfOperationJoiningLipsToTheMainBody = uniqueId(context, id);
                
                
                
                try{
                    opBoolean(context,idOfOperationJoiningLipsToTheMainBody,
                        {
                            operationType: BooleanOperationType.UNION,
                            tools: qUnion([mainBody, lipBodies]),
                            //targets: qNothing(),
                            //keepTools:false,
                            //targetsAndToolsNeedGrouping: false,
                            //matches: [],
                            //recomputeMatches: false
                        }
                    );
                }
                    
                //just in case the lipBodies are not in contact with the main body:
                returnValue = qUnion([returnValue, qCreatedBy(idOfOperationJoiningLipsToTheMainBody, EntityType.BODY)]);
                //this probably isn't necessary    
                   
                
            }
            
            var leftoverBodiesToBeDeleted = qSubtraction(
                qCreatedBy(id, EntityType.BODY),
                returnValue
            );
            
            //println("leftoverBodiesToBeDeleted: ");print(reportOnEntities(context, leftoverBodiesToBeDeleted,0,0));
            
            try {opDeleteBodies(context, uniqueId(context, id), {entities: leftoverBodiesToBeDeleted}); }
            
            // return qBodyType(qCreatedBy(idOfInitialBodyCreationOperation, EntityType.BODY), BodyType.SOLID);
            //return mainBody;
            //println("returnValue: " ~ reportOnEntities(context, returnValue,0,0));
            return returnValue;
               
        };

    addDefaultGettersAndSetters(this);
    return this;
}

export function new_bitHolderSegment(initialSettings) returns bitHolderSegment
{
    var this = new_bitHolderSegment();
    this[].set(initialSettings);
    return this;
}


// function bitHolderSegment_get_angleOfElevation() returns ValueWithUnits {return 30 * degree;}
// function bitHolderSegment_get_boreDiameter() returns ValueWithUnits {return 14 * millimeter;}


//===============================================================
export type bitHolder typecheck isBitHolder;
// a bitHolder is, primarily, a container for a set of bitHolderSegments,
// but it also defines the features that are not specific to one particuolar segment, like the mount holes, the 
// segment-to-segment spacing, etc.
// I am making bitHolder a box so that each bitHolderSegment can contain a reference to the bitHolder that contains it.

export predicate isBitHolder(value)
{
       value is box;
       value[].segments is array;
       for (var segment in value[].segments){segment is bitHolderSegment;}
}

export function new_bitHolder() returns bitHolder
{
    var this = new box({});
    this[].segments = [];
    this[].mountHole = 
        new_MountHole(
            {
                //these clearance diameters are appropriate for a UTS 8-32 pan-head screw.
                shankClearanceDiameter: 4.4958 * millimeter,
                headClearanceDiameter: 8.6 * millimeter,
                minimumAllowedClampingThickness: 3 * millimeter,
                clampingDiameter: 21 * millimeter,
                headClearanceHeight: 2.7 * millimeter
            }
        );
    this[].minimumAllowedExtentY = 12 * millimeter;
    
    this[].mountHolesGridSpacing = 1 * inch; //the mountHolesInterval will be constrained to be an integer multiple of this length.
    this[].get_xMinMountHolePositionX = function()
    {
        return 0.5 * this[].get_mountHolesGridSpacing() - 1.5 * millimeter; 
    };
    
    this[].mountHolesPositionZSpecifier = "grazeBottomOfSaddleBoreLip";
    // mountHolesPositionZSpecifier controls how the z position of the mount holes is determined.
    // the allowable values are 
    // 1) the string "grazeBottomOfSaddleBoreLip", which will cause the mount hole z position to be determined automatically so that the counterbore of the mount hole is tangent to the bottom lip of the socket bore lip.
    //  2)  the string "middle", which will cause the mount hole z position to be placed in the middle of the bit holder.
    //  3)  a valueWithUnits (e.g. -2 * millimeter), which will simply force the mount holes to be placed at the specified z coordinate (as measured in the frame of the bit holder).
    
    
    this[].get_mountHolePositions = function()
    {
        var mountHolePositions = makeArray(2);          
        
        
        //compute mountHolesPositionZ
        var mountHolesPositionZ;
        
         //strategy 1: hard coded value
         if(this[].get_mountHolesPositionZSpecifier() is ValueWithUnits)
         {
             mountHolesPositionZ = this[].mountHolesPositionZSpecifier;
         }
         
         //strategy 2: grazing the bottom saddle of the bore lip
         else if(this[].get_mountHolesPositionZSpecifier() == "grazeBottomOfSaddleBoreLip") 
         {
            mountHolesPositionZ = 
                max(
                    mapArray(
                        this[].get_segments(),
                        function(x){return dot(zHat, x[].get_bottomSaddlePointOfMouthFillet());}
                    )
                ) + this[].get_mountHole()[].get_headClearanceDiameter()/2;
        }
        
        //strategy 3: z midpoint
         else if(this[].get_mountHolesPositionZSpecifier() == "middle") 
         {
             mountHolesPositionZ = mean([this[].get_zMin(), this[].get_zMax()]);
         }
         else
         {
             mountHolesPositionZ = zeroLength;
         }
        
        //println("mountHolesPositionZ: " ~ toString(mountHolesPositionZ));		// mountHolesPositionZ
        
        //compute the x coordinates of the mount hole positions
        if( this[].get_extentX() < this[].get_mountHolesGridSpacing() ){return [];}
        
        // var mountHolesInterval = 
        //     floor(
        //         this[].get_extentX() - this[].get_mountHole()[].get_clampingDiameter() , 
        //         this[].get_mountHolesGridSpacing()
        //     );  
        var mountHolesInterval = 
            floor(
                this[].get_extentX() - this[].get_xMinMountHolePositionX() - this[].get_mountHole()[].get_clampingDiameter()/2 , 
                this[].get_mountHolesGridSpacing()
            );            
        mountHolePositions[0] = 
            vector(
                //this[].get_extentX()/2 - mountHolesInterval/2,
                this[].get_xMinMountHolePositionX(),
                zeroLength,
                mountHolesPositionZ
            );
        mountHolePositions[1] = mountHolePositions[0] + mountHolesInterval * xHat;            
        return  mountHolePositions; 
    };

    this[].get_extentX = function()
    {
        var extentX = new box(zeroLength);
        mapArray(this[].get_segments(),
            function(x)
            {
                extentX[] += x[].get_extentX();   
            }
        );
        return extentX[];            
    };
    
    this[].get_xMin = function()
    {
        return zeroLength;
    };
        
    this[].get_xMax = function()
    {
        return this[].get_xMin() + this[].get_extentX();
    };
    
    this[].makeExtentYOfAllSegmentsTheSame = function()
    {
        var commonMinimumAllowedExtentY;
        commonMinimumAllowedExtentY = 
            max(
                mapArray(
                    this[].get_segments(),
                    function(segment){return segment[].get_extentY();}
                )    
            );
        commonMinimumAllowedExtentY = max(commonMinimumAllowedExtentY, this[].get_minimumAllowedExtentY());    
        for(var segment in this[].get_segments())
        {
            segment[].set_minimumAllowedExtentY(commonMinimumAllowedExtentY);
        }
    };
    
    this[].get_zMax = function()
    {
        return 
            max(
                mapArray(
                    this[].get_segments(),
                    function(segment){return segment[].get_zMax();}
                )
            );
    };
    
    this[].get_zMin = function()
    {
        return 
            min(
                mapArray(
                    this[].get_segments(),
                    function(segment){return segment[].get_zMin();}
                )
            );
    };
    
    this[].get_extentZ = function()
    {
        return this[].get_zMax() - this[].get_zMin();
    };
    
    this[].get_yMin = function()
    {
        return 
            min(
                mapArray(
                    this[].get_segments(),
                    function(segment){return segment[].get_yMin();}
                )    
            );
    };
    
    this[].get_yMax = function()
    {
        return 
            max(
                mapArray(
                    this[].get_segments(),
                    function(segment){return segment[].get_yMax();}
                )    
            );
    };
    
    this[].get_extentY = function()
    {
        return this[].get_yMax() - this[].get_yMin();
           
    };
    
    this[].build = 
        function(context is Context, id is Id)
        {
            var returnBodies;
            var insertionPoint = vector(this[].get_xMin(),zeroLength,zeroLength);
            var segmentBodies = qNothing();
            for(var thisBitHolderSegment in this[].get_segments())
            {
                var mainBodyOfThisBitHolderSegment = thisBitHolderSegment[].build(context, uniqueId(context, id)); 
                var transformForThisSegment = transform(insertionPoint - thisBitHolderSegment[].get_xMin() * xHat);
                
                opTransform(
                    context, 
                    uniqueId(context, id),
                    {
                        "bodies": mainBodyOfThisBitHolderSegment,
                        "transform": transformForThisSegment
                    }
                );
                segmentBodies = qUnion([segmentBodies, mainBodyOfThisBitHolderSegment]);
                insertionPoint += thisBitHolderSegment[].get_extentX() * xHat;
                // debug(context, transformForThisSegment * thisBitHolderSegment[].get_bottomBoreCorner());
                // debug(context, transformForThisSegment * thisBitHolderSegment[].get_borePiercePoint());
                // debug(context, transformForThisSegment * thisBitHolderSegment[].get_topBoreCorner());
                // debug(context, transformForThisSegment * thisBitHolderSegment[].get_bottomSaddlePointOfMouthFillet());
            }
            returnBodies = segmentBodies;
            try silent{
                var idOfBoolean = uniqueId(context, id);
                opBoolean(
                    context, 
                    idOfBoolean, 
                    {
                        "tools" : segmentBodies,
                        "operationType" : BooleanOperationType.UNION
                    }
                );
                // a UNION does not create any bodies.  Rather, it modifies the first of the tool bodies, and destroys all the other tool bodies.
                // therefore, segmentBodies will return the resulting body of the boolean operation.
                // print( "qCreatedBy(idOfBoolean): "); debug(context, qCreatedBy(idOfBoolean));
                // println("numberOfEntitiesReturnedByQuery(context,segmentBodies): " ~ toString(numberOfEntitiesReturnedByQuery(context,segmentBodies)));		//                 numberOfEntitiesReturnedByQuery(context,segmentBodies)
                // returnBodies = qBodyType(qCreatedBy(idOfBoolean,EntityType.BODY), BodyType.SOLID);
                returnBodies = segmentBodies;
            }
            
            
            //cut the mount holes
            //TO DO: compute mountHole positions intelligently

            
            for(var mountHolePosition in this[].get_mountHolePositions())
            {
                try{
                        this[].get_mountHole()[].build(
                        context, 
                        uniqueId(context, id),
                        {
                            targetBodies:returnBodies, 
                            axis: line(mountHolePosition, yHat)
                        }
                    );
                }
            }
            return returnBodies;
        };
    
    addDefaultGettersAndSetters(this);
    this[].set_segments = 
        function(newValue)
        {
            this[].segments = newValue;
            for(var segment in this[].get_segments())
            {
                segment[].set_bitHolder(this);   
                //now that we are using a box to represent a bitHoilder, perhaps the above line should be replaced with something like:
                //segment[].bitHolder = x[].clone();    
            }
            
            this[].makeExtentYOfAllSegmentsTheSame();
            
            return newValue;
        };
    return this as bitHolder;
}

export function new_bitHolder(initialSettings) returns bitHolder
{
    var this = new_bitHolder();
    this[].set(initialSettings);
    return this;
}

