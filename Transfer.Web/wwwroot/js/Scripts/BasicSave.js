function DeleteFile(elem) {
    var row = $(elem).closest('.form-row');
    row.find('.file-upl').removeClass("d-none");
    row.find('.col-p-e').removeClass("d-none");
    row.find('.file-dwn').addClass("d-none");
    row.find('.col-p-u').addClass("d-none");
    row.find('.file-result-id').val('');
}
$(function () {
    $(".input-file").each(function () {
        var $el1 = $(this);
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
            allowedFileExtensions: ["jpg", "png", "gif", "pdf", "jpeg"],
        }).on("filebatchselected", function (event, files) {
            $el1.fileinput("upload");
        }).on('fileuploaded', function (event, data, previewId, index, fileId) {
            if (data.jqXHR.status == 200) {
                $el1.closest('.form-row').find('.file-result-name').val(data.response.fileName).removeClass('input-validation-error');
                $el1.closest('.form-row').find('.file-result-id').val(data.response.fileId);
            }
        });
    });
    $(".input-photo-file").each(function () {
        var $el1 = $(this);
        $el1.fileinput({
            browseClass: "add-photo-link",
            showPreview: false,
            showCaption: false,
            showRemove: false,
            showUpload: false,
            showCancel: false,
            browseLabel: "<i class=\"fas fa-plus-circle\"></i>",
            uploadUrl: "/File/UploadAnyFile",
            uploadAsync: true,
            layoutTemplates: { progress: '' },
            allowedFileExtensions: ["jpg", "png", "gif", "jpeg"],
        }).on("filebatchselected", function (event, files) {
            $el1.fileinput("upload");
        }).on('fileuploaded', function (event, data, previewId, index, fileId) {
            if (data.jqXHR.status == 200) {
                $el1.closest('.form-row').find('.file-result-id').val(data.response.fileId);
                $el1.closest('.form-row').find('.col-p-e').addClass("d-none");
                $el1.closest('.form-row').find('.col-p-u-i').css('background-image', 'url("/File/GetFile?fileId=' + data.response.fileId + '")');
                $el1.closest('.form-row').find('.col-p-u').removeClass("d-none");
            }
        });
    });
});
//# sourceMappingURL=BasicSave.js.map