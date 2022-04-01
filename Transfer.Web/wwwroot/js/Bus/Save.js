$(function () {
    $("#mainSaveForm").submit(function (event) {
        var notValid = false;
        $(".input-file.file-required").each(function () {
            var $el1 = $(this).closest('.form-row').find('.file-result-id');
            var $el2 = $(this).closest('.form-row').find('.file-result-name');
            //console.log($el1.val(), $el2.val());
            if (!$el1.val()) {
                $el2.addClass('input-validation-error');
                notValid = true;
            }
        });
        if (notValid) {
            event.preventDefault();
        }
    });
    function DoSometh() {
        alert('do');
    }
});
//# sourceMappingURL=Save.js.map