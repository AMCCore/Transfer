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
function LoginStart(e) {
    $('#LoginPopUp :input').prop("disabled", true);
}
function LoginFailure(e) {
    $('#LoginPopUp :input').prop("disabled", false);
}
function LoginSuccessRefresh(e) {
    if (e.redirect) {
        window.location.href = e.redirect;
    }
}
function DisableCheckboxes(e) {
    var elem = $(e);
    if (elem.is(':checked')) {
        $('.ordering input:checkbox').removeAttr('checked');
        elem.attr('checked', 'checked');
        elem.closest('form').submit();
    }
    else {
        elem.closest('form').submit();
    }
}
$(function () {
});
//# sourceMappingURL=site.js.map