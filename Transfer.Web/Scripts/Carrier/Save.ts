$(() => {
    $("#mainSaveForm").submit(function (event) {
        let notValid = false;
        $(".input-file.file-required").each(function () {
            let $el1 = $(this).closest('.form-row').find('.file-result-id');
            let $el2 = $(this).closest('.form-row').find('.file-result-name');

            if (!$el1.val()) {
                $el2.addClass('input-validation-error');
                notValid = true;
            }
        });
        if (notValid) {
            event.preventDefault();
        }
    });
});