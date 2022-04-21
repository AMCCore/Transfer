$(() => {
    $(".basicAutoComplete").select2({
        minimumInputLength: 3,
        placeholder: "Select a state",
        allowClear: true
    });
});