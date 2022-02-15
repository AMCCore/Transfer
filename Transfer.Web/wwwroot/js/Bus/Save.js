function DeleteFile(elem) {
    var row = $(elem).closest('.form-row');
    row.find('.file-upl').removeClass("d-none");
    row.find('.file-dwn').addClass("d-none");
}
$(function () {
    var $el1 = $("#input-id");
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
            $el1.closest('.form-row').find('.file-result-name').val(data.response.fileName);
            $el1.closest('.form-row').find('.file-result-id').val(data.response.fileId);
        }
    });
});
//# sourceMappingURL=Save.js.map