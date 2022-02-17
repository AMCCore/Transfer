function DeleteFile(elem) {
    let row = $(elem).closest('.form-row');
    row.find('.file-upl').removeClass("d-none");
    row.find('.file-dwn').addClass("d-none");
    row.find('.file-result-id').val('');
}

$(() => {
    $("#mainSaveForm").submit(function (event) {
        let notValid = false;
        $(".input-file").each(function () {
            let $el1 = $(this).closest('.form-row').find('.file-result-id');
            let $el2 = $(this).closest('.form-row').find('.file-result-name');
            if ($el1.val() && !$el2.val()) {
                $el2.addClass('input-validation-error');
                notValid = true;
            }
        });
        if (notValid) {
            event.preventDefault();
        }
    });


    //let $el1 = $("#input-id");
    $(".input-file").each(function() {
        let $el1 = $(this);
        //console.log($el1);

        $el1.fileinput({
            browseClass: "button-one",
            showPreview: false,
            showCaption: false,
            showRemove: false,
            showUpload: false,
            showCancel: false,
            browseLabel: "Загрузить",
            uploadUrl: "/File/UploadAnyFile",
            uploadAsync: true,
            layoutTemplates: { progress: '' },
            allowedFileExtensions: ["jpg", "png", "gif"],
        }).on("filebatchselected", function (event, files) {
            $el1.fileinput("upload");
        }).on('fileuploaded', function (event, data, previewId, index, fileId) {
            if (data.jqXHR.status == 200) {
                $el1.closest('.form-row').find('.file-result-name').val(data.response.fileName).removeClass('input-validation-error');
                $el1.closest('.form-row').find('.file-result-id').val(data.response.fileId);
            }
        });
    });
});
