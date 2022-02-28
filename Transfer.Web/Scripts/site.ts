let w;
w = window;

w.jQuery = $;
w.$ = $;

function TSButton() {
    let name = "Fred";
    let ыname = "Smittt";
    $('.ts-example').html(greeter(user));
}

class Student {
    fullName;
    constructor(firstName, middleInitial, lastName) {
        this.fullName = firstName + " z " + middleInitial + " " + lastName;
    }
}

function greeter(person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}

let user = new Student("Fred", "M.", "Smith");

function SomeButtonZero() {
    let v: any = jQuery.fn.jquery.toString();
    let name = "Zusul";
    
    alert(name + ' Version:' + v);
}

function LoginStart(e) {
    $('#LoginPopUp :input').prop("disabled", true);
    $('#the-msg').html('');
}

function LoginFailure(e) {
    $('#LoginPopUp :input').prop("disabled", false);
    $('#the-msg').html(e.responseText);
}


function LoginSuccessRefresh(e) {
    if (e.redirect) {
        window.location.href = e.redirect;
    }
}

function DisableCheckboxes(e) {
    let elem = $(e);
    if (elem.is(':checked')) {
        $('.ordering input:checkbox').removeAttr('checked');
        elem.attr('checked', 'checked');
        elem.closest('form').submit();
    }
    else {
        elem.closest('form').submit();
    }
}

$(() => {
});
