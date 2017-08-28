var vm = new Vue({
    el: '#app',
    data: {
        where: {
            RowStatus: '1',
            Time: [],
            Status: '',
            UserName: ''
        },
        currentPage: 1,
        list: [],
        total: 0,
        loading: false,
        dialogVisible: false,
        dialogLoading: false,
        excelVisible: false,
        oldInfo: {
            OrderState: '',
            RowStatus: '',
            DeleteDescribe: ''
        },
        newInfo: {
            OrderState: '',
            RowStatus: '',
            DeleteDescribe: ''
        },
        fileList: [],
        btnLoading: false
    },
    methods: {
        getpage: function (currentPage) {
            var that = this;
            that.where.CurrentPage = currentPage
            if (that.where.Time.length > 1 && that.where.Time[0] != null && that.where.Time[1] != null) {
                that.where.Time = [new Date(that.where.Time[0]).Format('yyyy-MM-dd hh:mm:ss'), new Date(that.where.Time[1]).Format('yyyy-MM-dd hh:mm:ss')]
            }
            that.loading = true
            ajax('/CsOrder/GetCsOrderPage', that.where, function (data) {
                that.list = data.data
                that.currentPage = currentPage
                that.total = data.total
                that.loading = false
            })
        },
        detailRow: function (id) {
            var that = this;
            if (!that.dialogVisible) {
                that.dialogVisible = true
            }
            if (id === that.oldInfo.OrderId) {
                return
            }
            that.dialogLoading = true
            ajax('CsOrder/GetCsOrderInfo', {
                id
            }, function (data) {
                that.oldInfo = clone(data)
                that.newInfo = clone(data)
                that.dialogLoading = false
            })
        },
        closeInfo: function () {
            this.newInfo = clone(this.oldInfo)
            this.dialogVisible = false
        },
        updateInfo: function (id) {
            var that = this
            that.dialogLoading = true
            ajax('CsOrder/UpdateCsOrder', {
                id: id,
                rowStatus: that.newInfo.RowStatus,
                deleteDate: that.newInfo.DeleteDate,
                deleteDescribe: that.newInfo.DeleteDescribe,
                orderState: that.newInfo.OrderState,
                delivery: that.newInfo.OrderDelivery
            }, function (data) {
                var type, title
                if (data.code === 1) {
                    type = 'success'
                    title = '更新成功'
                } else if (data.code === 2) {
                    type = 'warning'
                    title = '更新无结果'
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: data.data
                    })
                    return
                }
                that.$notify({
                    title: title,
                    message: data.data,
                    type: type
                });
                that.oldInfo = {}
                that.newInfo = {}
                that.getpage(that.currentPage)
                that.dialogVisible = false
                that.dialogLoading = false
            })
        },
        exportExcel: function () {
            var that = this;
            if (that.where.Time.length > 1 && that.where.Time[0] != null && that.where.Time[1] != null) {
                that.where.Time = [new Date(that.where.Time[0]).Format('yyyy-MM-dd hh:mm:ss'), new Date(that.where.Time[1]).Format('yyyy-MM-dd hh:mm:ss')]
            }
            that.loading = true
            ajax('/CsOrder/ExportCsOrder', that.where, function (data) {
                if (data.code === 1) {
                    window.open(data.data, '_blank')
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: data.data
                    })
                }
                that.loading = false
            })
        },
        importExcel: function () {
            var that = this
            if (that.fileList.length <= 0) {
                that.$notify.error({
                    title: '错误',
                    message: '请上传Excel文件'
                })
                return
            }
            that.btnLoading = true
            ajax('/CsOrder/ImportCsOrder', {
                path: that.fileList[0].path
            }, function (data) {
                if (data.code === 1) {
                    that.$notify({
                        title: '成功',
                        message: data.data,
                        type: 'success'
                    });
                    that.excelVisible = false
                    that.getpage(that.currentPage)
                } else {
                    that.$notify.error({
                        title: '错误',
                        message: data.data
                    })
                }
                that.btnLoading = false
            })
        },
        uploadSuccess: function (file, obj) {
            this.fileList = [{
                name: obj.name,
                url: '../excelicon.png',
                path: file.Data
            }]
        },
        uploadRemove: function (file, fileList) {
            this.fileList = []
        },
        uploadBefore: function (file) {
            if (file.name.indexOf('.xls') < 0) {
                this.$notify.error({
                    title: '错误',
                    message: '请选择Excel文件后缀为: xls或xlsx'
                })
                return false
            }
        }
    },
    mounted: function () {
        if (req['userId'] > 0) {
            this.where.UserName = req['userId']
        }
        this.getpage(1)
    },
    computed: {
        infoIsChange: function () {
            var b
            b = this.oldInfo.OrderState != this.newInfo.OrderState ||
                this.oldInfo.RowStatus != this.newInfo.RowStatus ||
                this.oldInfo.DeleteDescribe != this.newInfo.DeleteDescribe ||
                this.oldInfo.OrderDelivery != this.newInfo.OrderDelivery
            return b
        }
    },
    watch: {
        'newInfo.RowStatus': function (newValue) {
            if (this.oldInfo.RowStatus == 1) {
                this.newInfo.DeleteDate = new Date().Format("yyyy-MM-dd hh:mm:ss")
            } else {
                this.newInfo.DeleteDate = new Date(this.oldInfo.DeleteDate).Format("yyyy-MM-dd hh:mm:ss")
            }
        },
        excelVisible: function (newValue) {
            if (newValue) {
                setTimeout(function () {
                    document.getElementsByName('file')[0].setAttribute('accept', "application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                }, 500)
                this.fileList = []
            }
        }
    }
})