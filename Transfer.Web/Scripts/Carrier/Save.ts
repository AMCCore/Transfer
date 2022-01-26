$(() => {
    $("#LogoFile111").fileinput({
        overwriteInitial: true,
        maxFileSize: 1500,
        showClose: false,
        
        //showBrowse: false,
        //browseOnZoneClick: true,
        //removeLabel: '',
        //removeIcon: '<i class="bi-x-lg"></i>',
        //removeTitle: 'Cancel or reset changes',
        elErrorContainer: '#kv-avatar-errors-2',
        msgErrorClass: 'alert alert-block alert-danger',
        //defaultPreviewContent: '<img src="/samples/default-avatar-male.png" alt="Your Avatar"><h6 class="text-muted">Click to select</h6>',
        //layoutTemplates: { main2: '{preview} ' + btnCust + ' {remove} {browse}' },
        allowedFileExtensions: ["jpg", "png", "gif"],
        allowedPreviewTypes: ["image"],

        showUpload: false,
        showPreview: false,
        showCaption: false,
        showRemove: true,
        theme: "fas",
    });

    let $el1 = $("#LicenceFile");

    $el1.fileinput({
        browseClass: "button-one",
        showPreview: false,
        showCaption: false,
        showRemove: false,
        showUpload: false,
        browseLabel: "Загрузить",
        uploadUrl: "/File/UploadFile",
        uploadAsync: true,
    }).on("filebatchselected", function (event, files) {
        $el1.fileinput("upload");
    }).on('filebatchuploadcomplete', function (event, files, extra) {
        console.log('File batch upload complete', files, extra);
    });
});