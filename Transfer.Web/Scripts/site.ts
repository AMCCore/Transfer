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

function ConfirmPopup(link: string | undefined, title: string | undefined, body: string | undefined) {
    if (link === undefined || link.length < 1)
        return;
    let pop = $('#confirm-action');

    if (title && title.length > 1) {
        pop.find('h4').html(title);
    } else {
        pop.find('h4').html('Подтвердите действие');
    }

    if (body && body.length > 1) {
        pop.find('.body').html(body);
    } else {
        pop.find('.body').html('');
    }

    pop.find("a.btn-confirm").attr("href", link);

    pop.modal('show');
}

$(() => {
    $(".phone_mask").mask('+7 (000) 000-0000', { placeholder: "+7 (___) ___-____" });
    $('.email_mask').mask("A", {
        translation: {
            "A": { pattern: /[\w@\-.+]/, recursive: true }
        }
    });
    $(".inn_mask").mask('000000000000');
    $(".ogrn_mask").mask('0000000000000');
    $(".bik_mask").mask('000000000');
    $(".numaccount_mask").mask('00000000000000000000');

});
