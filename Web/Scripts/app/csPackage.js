var vm = new Vue({
    el: '#app',
    data: {
        where: {
            PackageName: '',
            PackageState: ''
        },
        list: [],
        loading: false,
        info: {

        },
        dialogVisible: false,
        submitLoad: false
    },
    methods: {
        getList: function () {
            var that = this
            that.loading = true
            ajax('CsPackage/GetCsPackageList', that.where, function (data) {
                that.list = data.data
                that.loading = false
            })
        },
        getInfo: function (id) {
            var that = this
            that.loading = true
            ajax('CsPackage/GetCsPackageInfo', { id: id }, function (data) {
                that.info = data.data
                that.info.PackageState = data.data.PackageState + ''
                that.loading = false
                if (data.code === 0) {
                    that.$notify({
                        title: '失败',
                        message: data.data,
                        type: 'error'
                    });
                    return
                }
                that.dialogVisible = true
            })
        },
        subInfo: function () {
            var that = this
            that.submitLoad = true
            ajax('CsPackage/SubmitCsPackageInfo', that.info, function (data) {
                that.submitLoad = false
                that.dialogVisible = false
                that.getList()
                if (data.code === 1) {
                    that.$notify({
                        title: '成功',
                        message: data.data,
                        type: 'success'
                    });
                    return
                } else if (data.code === 2) {
                    that.$notify({
                        title: '警告',
                        message: data.data,
                        type: 'warning'
                    });
                    return
                }
                that.$notify({
                    title: '失败',
                    message: data.data,
                    type: 'error'
                });
            })
        }
    },
    mounted: function () {
        this.getList()
    }
})