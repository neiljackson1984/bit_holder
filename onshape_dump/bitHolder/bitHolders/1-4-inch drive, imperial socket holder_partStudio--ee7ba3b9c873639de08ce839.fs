FeatureScript 1540;
import(path : "onshape/std/geometry.fs", version : "1540.0");

annotation { "Default Units" : ["degree", "kilogram", "millimeter"] }
export function main()
{
    return build({});
}

export function build()
{
    return buildPrivate({});
}

export function build(configuration is map)
{
    return buildPrivate(configuration);
}

const buildPrivate = definePartStudio(function(context is Context, configuration is map, lookup is function)
    precondition
    {
    }
    {
        const id is Id = newId();
        annotation { 'unused' : true }
        var features = {};
        features.Fe3kgTkIQcUex9Y_0 = function(id)
            {
                annotation { 'unused' : true }
                var features = features;
                if (true)
                {
                    {
                    }
                    annotation { "Feature Name" : "#nameOfSelectedCannedDesign" }
                    e9f05a85f95bf68be1c454008::m3fe83b491664242c37fd48a9::bitHolder_feature(context, id + "Fe3kgTkIQcUex9Y_0", { "nameOfSelectedCannedDesign" : "1/4-inch drive, imperial sockets holder", "cannedDesignChoices" : [{ "name" : "1/4-inch drive, imperial sockets holder", "displayName" : "(*)  1/4-inch drive, imperial sockets holder", "selected" : true }, { "name" : "1/4-inch drive, metric sockets holder", "displayName" : "( )  1/4-inch drive, metric sockets holder", "selected" : false }, { "name" : "1/4-inch hex shank drill bits holder", "displayName" : "( )  1/4-inch hex shank drill bits holder", "selected" : false }, { "name" : "3/8-inch drive, imperial sockets holder", "displayName" : "( )  3/8-inch drive, imperial sockets holder", "selected" : false }, { "name" : "3/8-inch drive, metric sockets holder", "displayName" : "( )  3/8-inch drive, metric sockets holder", "selected" : false }, { "name" : "bondhus_hex_drivers_holder", "displayName" : "( )  bondhus_hex_drivers_holder", "selected" : false }], "refreshCannedDesignsRequested" : false, "changeCount" : { 'value' : try(14.0), 'expression' : "14.0" }.value });
                }
            };
        try(features.Fe3kgTkIQcUex9Y_0(id));
        return context;
    }, millimeter, {});
