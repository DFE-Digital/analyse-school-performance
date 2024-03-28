document.addEventListener('alpine:init', () => {
    Alpine.data('app-chart', () => ({
        isGraphView: true,

        init() {
            // Removes the element with ref="nojs" from the DOM
            // See ./Chart.cshtml for more details
            this.$refs.nojs.remove();
        },

        isTableView() {
            return !this.isGraphView;
        },

        toggleGraphView() {
            this.isGraphView = !this.isGraphView;
        }
    }))
})