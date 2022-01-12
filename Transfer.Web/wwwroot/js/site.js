var w;
w = window;
w.jQuery = $;
w.$ = $;
function TSButton() {
    var name = "Fred";
    $('.ts-example').html(greeter(user));
}
var Student = /** @class */ (function () {
    function Student(firstName, middleInitial, lastName) {
        this.fullName = firstName + " z " + middleInitial + " " + lastName;
    }
    return Student;
}());
function greeter(person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}
var user = new Student("Fred", "M.", "Smith");
function SomeButtonZero() {
    var v = jQuery.fn.jquery.toString();
    var name = "Zusul";
    alert(name + ' Version:' + v);
}
function LoginSuccessRefresh(e) {
    if (e.redirect) {
        window.location.href = e.redirect;
    }
}
//# sourceMappingURL=site.js.map