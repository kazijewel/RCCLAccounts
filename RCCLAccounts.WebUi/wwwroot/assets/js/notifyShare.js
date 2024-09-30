function successNotify(data) {
    new PNotify({
        title: 'Success!',
        text: data,
        type: 'success'
    });
}
function errorNotify(data) {
    new PNotify({
        title: 'Error!',
        text: data,
        type: 'error'
    });
}
function warningNotify(data) {
    new PNotify({
        title: 'Warning!',
        text: data
    });
}