function HomeViewModel(app, dataModel) {
    var self = this;

    // HomeViewModel currently does not require data binding, so there are no visible members.
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
