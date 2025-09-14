


$(document).ready(function () {
    loadFiscalYearData();
});

function loadFiscalYearData() {
    $.ajax({
        url: '/FiscalYearCloseing/getFiscalYearDate',
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res && res.Opening && res.Closing) {
                const fiscalRange = res.Opening + ' to ' + res.Closing;
                updateFiscalYearUI(fiscalRange);
            } else {
                showDefaultFiscalYear();
            }
        },
        error: function () {
            showDefaultFiscalYear();
        }
    });
}

// Helper methods
function updateFiscalYearUI(fiscalYearData) {
    $('#currentFiscalYearDisplay').text(fiscalYearData);
    $('#periodText').text('Period: ' + fiscalYearData);
}

function showDefaultFiscalYear() {
    $('#currentFiscalYearDisplay').text('2024-07-01 to 2025-06-30');
    $('#periodText').text('Period: 2024-07-01 to 2025-06-30');
}









