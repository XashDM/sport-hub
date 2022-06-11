class DateClass {
    activeItem = {
    Category: null,
    Subcategory: null,
    Team: null,
    };
    Tree = null;
    SaveInMoment = false;
    addedItem = [];

    parentFor(type) {
        if (type == "Category") {
            return null;
        } else if (type == "Subcategory") {
            return this.activeItem["Category"];
        } else if (type == "Team") {
            return this.activeItem["Subcategory"];
        }
    }
    getTree() {
        return this.Tree;
    }
    getTeamofSubcategory() {
            let result;
            parent = this.parentFor('Team');
            if (parent == null) {
                return [];
            }
            if (parent.children == null) {
                let parentsId = parent.id;
                $.ajax({
                    url: '/GetChildren',
                    type: "GET",
                    data: { ItemId: parentsId },
                    async: false,
                    headers: {
                        RequestVerificationToken: $(
                            'input:hidden[name="__RequestVerificationToken"]'
                        ).val(),
                    },
                }).done(function (date) {
                    parent.children = date;
                    result = date;
                });
            }
            else {
                result = parent.children
            }
            console.log(this.Tree);
            return result;
    }
    
    getCategory() {
        let result;
        if (this.Tree == null) {
            $.ajax({
                url: '/GetRoot',
                data: $("#tab"),
                async: false,
            }).done(function (date) {
                result = date;
                this.Tree = date;
            });
        }
        else {
            result = this.Tree
        }
        this.Tree = result;

        return result;
    }

    getSubcategoryofCategory() {
        let result;
        parent = this.parentFor("Subcategory");
        if (parent == null) {
        return [];
        }

        if (parent.children == null) {
            let parentsId = parent.id;
            $.ajax({
                url: '/GetChildren',
                type: "GET",
                data: { ItemId: parentsId },
                async: false,
                headers: {
                    RequestVerificationToken: $(
                        'input:hidden[name="__RequestVerificationToken"]'
                    ).val(),
                },
            }).done(function (date) {

                parent.children = date;
                result = date;
            });
        }
        else {
            result = parent.children
        }
        return result;
    }

}
class ExtendDateClass extends DateClass {

    getTeamofSubcategory(parent) {
        let result;
        if (parent == null) {
            return [];
        }
        if (parent.children == null) {
            let parentsId = parent.id;
            $.ajax({
                url: '/GetChildren',
                type: "GET",
                data: { ItemId: parentsId },
                async: false,
                headers: {
                    RequestVerificationToken: $(
                        'input:hidden[name="__RequestVerificationToken"]'
                    ).val(),
                },
            }).done(function (date) {
                parent.children = date;
                result = date;
            });
        }
        else {
            result = parent.children
        }
        console.log(this.Tree);
        return result;
    }
    
    getCategory() {
    let result;
    if (this.Tree == null) {
        $.ajax({
            url: '/GetRoot',
            data: $("#tab"),
            async: false,
        }).done(function (date) {
            result = date;
            this.Tree = date;
        });
    }
    else {
        result = this.Tree
    }
    console.log(this.Tree);
    return result;
    }

    getSubcategoryofCategory(parent) {
    let result;

    if (parent == null) {
        return [];
    }

    if (parent.children == null) {
        let parentsId = parent.id;
        $.ajax({
            url: '/GetChildren',
            type: "GET",
            data: { ItemId: parentsId },
            async: false,
            headers: {
                RequestVerificationToken: $(
                    'input:hidden[name="__RequestVerificationToken"]'
                ).val(),
            },
        }).done(function (date) {

            parent.children = date;
            result = date;
        });
    }
    else {
        result = parent.children
    }
    return result;
    }
    constructor() {
        super();
        this.getCategory();
    }
}
