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
        submitLoad: false,
        rules: {
            PackageName: [{
                required: true,
                message: '请输入套餐名称',
                trigge: 'blur'
            }],
            PackagePrice: [
                { required: true, message: '价格不能为空' },
                { type: 'number', message: '价格必须为数字值' }
            ]
        }

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
        add: function () {
            this.info.PackageId = 0
            this.dialogVisible = true
            this.info = {
                PackageName: '',
                PackageNumber: '',
                PackagePrice: 0,
                OperationDate: '',
                PackageState: '1',
                DeleteDescribe: '',
                PackageId: 0
            }
        },
        getInfo: function (id) {
            var that = this
            that.loading = true
            ajax('CsPackage/GetCsPackageInfo', { id: id }, function (data) {
                that.info = data.data
                that.info.PackageState = data.data.PackageState + ''
                that.info.PackagePrice = data.data.PackagePrice;
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
            that.$refs['info'].validate((valid) => {
                if (valid) {
                    //验证成功
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
                } else {
                    that.$notify({
                        title: '失败',
                        message: '请按要求填写表单',
                        type: 'error'
                    });
                    that.submitLoad = false;
                }
            });

        }
    },
    mounted: function () {
        this.getList()
    }
})