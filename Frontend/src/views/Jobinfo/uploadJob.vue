<template>
    <div class="home" style="background-color: #F0F2F5;height: 800px;">
        <div style="height:10px">
        </div>
        <!-- <el-row :gutter="10">
            <el-col :span="14" :offset="5">
                <ElCard>
                    <el-row style="margin-top: 10px;">
                        <span>岗位名称: </span>
                        <ElInput></ElInput>
                    </el-row>

                    <el-row style="margin-top: 10px;">
                        <span>岗位信息: </span>
                        <ElInput></ElInput>
                    </el-row>

                    <el-row style="margin-top: 10px;">
                        <span>最低工作年限: </span>
                        <ElInput></ElInput>
                    </el-row>
                    <el-row style="margin-top: 10px;">
                        <span>最低学历: </span>
                        <ElInput></ElInput>
                    </el-row>
                    <el-row style="margin-top: 10px;">
                        <ElButton type="primary" @click="onSubmit"> 保存修改 </ElButton>
                    </el-row>

                </ElCard>
            </el-col>
        </el-row> -->

        <el-row style="height: 50px;">
            <el-col :span="24" style="margin-left: 20px;">
                <span class="title">上传岗位</span>
            </el-col>
        </el-row>
        <div style="margin-left: 20px;">
            <el-row style="margin-bottom: 20px;">
                <el-col>
                    <div style="margin-bottom: 10px;font-size: 16px;font-weight: bold;">岗位名称</div>
                    <el-input v-model="jobName" style="width: 97.5%;" placeholder="请输入岗位名称"></el-input>
                </el-col>
            </el-row>
            <el-row :gutter="10" style="margin-bottom: 20px;">
                <el-col :span="12">
                    <div style="margin-bottom: 10px;font-size: 16px;font-weight: bold;">最低学历要求</div>
                    <el-select v-model="minimumEducationLevel" style="width: 95%;">
                        <el-option v-for="item in EducationOptions" :key="item" :label="item" :value="item" />
                    </el-select>
                </el-col>
                <el-col :span="12">
                    <div style="margin-bottom: 10px;font-size: 16px;font-weight: bold;">招聘时间</div>
                    <el-select style="width: 95%;"></el-select>
                </el-col>
            </el-row>

            <el-row :gutter="10" style="margin-bottom: 20px;">
                <el-col :span="12">
                    <div style="margin-bottom: 10px;font-size: 16px;font-weight: bold;">薪资待遇</div>
                    <el-input v-model="salary" style="width: 97.5%;" placeholder="请输入薪资待遇"></el-input>
                </el-col>
                <el-col :span="12">
                    <div style="margin-bottom: 10px;font-size: 16px;font-weight: bold;">要求工作经验</div>
                    <el-select v-model="minimumWorkYears" style="width: 95%;">
                        <el-option v-for="item in Array.from({ length: 100 }, (_, i) => 1 + (i))" :value="item"></el-option>
                    </el-select>
                </el-col>
            </el-row>

            <el-row :gutter="10" style="margin-bottom: 20px;">
                <el-col :span="12">
                    <div style="margin-bottom: 10px;font-size: 16px;font-weight: bold;">技能要求</div>
                    <el-tag v-for="sc in jobKeywords" class="ml-2" style="margin-right: 5px;margin-top: 10px">{{ sc }}</el-tag>
                    <el-button type="primary" plain @click="addTag"> 添加标签 + </el-button>
                </el-col>
            </el-row>

            <el-row :gutter="10" style="margin-bottom: 30px;">
                <el-col :span="24">
                    <div style="margin-bottom: 10px;font-size: 16px;font-weight: bold;">岗位描述</div>
                    <div style="width: 97.5%;">
                        <el-card class="discription">
                            <template #header>
                                <el-row>
                                    <!-- <el-col :span="1"> <svg-icon icon-class="documentation"></svg-icon> </el-col>
                                    <el-col :span="1"> <svg-icon icon-class="documentation"></svg-icon> </el-col>
                                    <el-col :span="1"> <svg-icon icon-class="documentation"></svg-icon> </el-col>
                                    <el-divider direction="vertical"></el-divider>
                                    <el-col :span="1"> <svg-icon icon-class="documentation"></svg-icon> </el-col>
                                    <el-col :span="1"> <svg-icon icon-class="documentation"></svg-icon> </el-col>
                                    <el-col :span="1"> <svg-icon icon-class="documentation"></svg-icon> </el-col>
                                    <el-col :span="1"> <svg-icon icon-class="documentation"></svg-icon> </el-col> -->
                                    <span style="font-size:14px;font-weight:bolder">上传岗位的详细描述, 通常分为岗位职责和任职要求</span>
                                </el-row>
                            </template>
                            <el-input v-model="jobDetails" :autosize="{ minRows: 8, maxRows: 12 }" type="textarea"
                                placeholder="请输入岗位描述" />
                        </el-card>
                    </div>

                </el-col>
            </el-row>

            <el-row>
                <el-col :span="4" :offset="4">
                    <el-button size="large" @click="reset">
                        <span style="width: 80px;">重 置</span>
                    </el-button>
                </el-col>

                <el-col :span="4" :offset="8">
                    <el-button type="primary" size="large" @click="submit">
                        <span style="width: 80px;">提 交</span>
                    </el-button>
                </el-col>
            </el-row>
        </div>

        <el-dialog v-model="isVisible" title="添加标签" width="30%" :before-close="handleClose" style="margin-top: 200px;">

            <el-input v-model="tag" type="textarea" placeholder="请输入岗位标签" maxlength="12" show-word-limit id="tag">

            </el-input>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="isVisible = false">取消</el-button>
                    <el-button type="primary" @click="handleSubmit">
                        提交
                    </el-button>
                </span>
            </template>


        </el-dialog>
    </div>
</template>

<script>
import { mapStores } from 'pinia'
import { useUser } from '../../stores/user';
import pinia from '../../stores';
import axios from 'axios';
export default {
    data() {
        return {
            EducationOptions: ['专科', '本科', '研究生'],
            isVisible: false,
            tag: '',
            salary: '',

            jobName: '',
            minimumEducationLevel: '',
            jobKeywords: [],
            minimumWorkYears: null,
            jobDetails: ''

        }
    },
    computed: {
        ...mapStores(useUser),
    },
    methods: {
        addTag() {
            this.isVisible = true
            console.log('添加标签')
        },
        handleClose() {

        },
        handleSubmit(){
            this.jobKeywords.push(document.getElementById('tag').value)
            console.log(this.jobKeywords)
            this.isVisible = false
            this.tag = ''
        },
        reset() {
            this.jobName = ''
            this.minimumEducationLevel = ''
            this.jobKeywords = []
            this.minimumWorkYears = null
            this.jobDetails = ''
        },
        submit() {
            axios.post('http://localhost:5177/api/Job/uploadJobs', {
                userId: this.userStore.userId,
                jobName: this.jobName,
                jobDetails: this.jobDetails,
                jobKeywords: this.jobKeywords,
                minimumWorkYears: this.minimumWorkYears,
                minimumEducationLevel: this.minimumEducationLevel
            }).then((res) => {
                console.log(res)
                this.$router.push({path: '/job-info'})
            })
        }
    },
}
</script>
    
<style scoped>
.title {
    padding-top: 25px;
    font-size: 24px;
    color: gray;
}

/* .discription {
    > :deep(.el-textarea__inner) {
        background-color: #bfcbd9;
    }
} */

:deep(.el-card__body) {
    padding: 5px;
}

:deep(.el-divider--vertical) {
    display: inline-block;
    width: 1px;
    height: 20px;
    margin: 0 30px 0 0;
    vertical-align: middle;
    position: relative;
    border-left: 2px gray var(--el-border-style);
}
</style>
    