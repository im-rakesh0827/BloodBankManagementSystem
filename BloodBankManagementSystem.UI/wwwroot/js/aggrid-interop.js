window.agGridInterop = {
    renderGrid: function (elementId, columnDefs, rowData) {
        const gridOptions = {
            columnDefs: columnDefs,
            rowData: rowData,
            animateRows: true,
            pagination: true,
            paginationPageSize: 10,
            defaultColDef: {
                sortable: true,
                filter: true,
                resizable: true
            }
        };

        const eGridDiv = document.querySelector("#" + elementId);
        if (eGridDiv) {
            new agGrid.Grid(eGridDiv, gridOptions);
        } else {
            console.warn("AG Grid element not found: #" + elementId);
        }
    }
};
