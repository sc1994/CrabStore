var vm = new Vue({
    el: '#app',
    data: {
        where: {
            RowStatus: '1',
            Time: []
        },
        currentPage: 1
    },
    methods: {
        getpage: function (currentPage) {
            var that = this;
            that.where.CurrentPage = currentPage
            if (that.where.Time.length > 1 && that.where.Time[0] != null && that.where.Time[1] != null) {
                that.where.Time = [that.where.Time[0].Format('yyyy-MM-dd'), that.where.Time[1].Format('yyyy-MM-dd')]
            }
            ajax('/CsOrder/GetCsOrderPage', that.where, function (data) {
                console.log(data)
            })
        }
    }
})