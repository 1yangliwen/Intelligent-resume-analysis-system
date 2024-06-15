<template>
    <div>
    <div class="home" style="background-color: #F0F2F5;height: 750px;">
        <div style="height:10px">
        </div>
        <el-row :gutter="20">
            <el-col :span="24" style="margin-left: 10px;">
                <!-- action = "'http://localhost:5168/api/ResumeView/UploadResume' + '?userId=' + userStore.userId" -->
                <ElUpload action="" :http-request="uploadFile" drag style="width: 100%;align-items: center;"
                    :on-change="afterUpload" v-loading="isLoading" element-loading-text="Loading..."
                    :element-loading-spinner="svg" element-loading-svg-view-box="-10, -10, 50, 50"
                    element-loading-background="rgba(122, 122, 122, 0.8)" limit="3" >

                    <el-icon class="el-icon--upload"><upload-filled /></el-icon>
                    <div class="el-upload__text">
                        拖拽文件到此或 <em>点击上传</em>
                    </div>
                </ElUpload>
            </el-col>
        </el-row>

        <el-row :gutter="30">
            <el-col :span="24" style="margin-left: 10px;" y>
                <ElTable :data="resumeList" :border="true" style="width: 100%;" max-height="520px" @cell-click="clickItem">
                    <el-table-column prop="name" label="姓名" />
                    <el-table-column prop="age" label="年龄" />
                    <el-table-column prop="highestEducation" label="学历" />
                    <el-table-column prop="graduatedFrom" label="毕业院校" />
                    <el-table-column prop="phoneNumber" label="联系方式" />
                    <el-table-column prop="jobIntention" label="求职意向" />
                    <el-table-column prop="matchingPosition" label="匹配岗位" />
                    <ElPagination layout="prev, pager, next" :total="3" />
                </ElTable>

            </el-col>

        </el-row>

    </div>
</div>
</template>

<script>
import { ElRow, ElCol, ElUpload, ElTable, ElPagination, ElInput, ElSelect, ElForm, ElButton, ElDrawer, ElOption } from 'element-plus';


import axios from 'axios';
import * as docx from 'docx-preview';
import { renderAsync } from 'docx-preview';
import JSZip from 'jszip';
import Cookies from 'js-cookie'

import { getResumeParser } from '../../api/resume'

// 引入所有Store
import { useUser } from '../../stores/user';
import { useFileStore } from '../../stores/file'
import { useResumeDetail } from '../../stores/resume'

import pinia from '../../stores';
import { mapState, mapStores } from 'pinia';

import resumeDetails from './ResumeComponent/resumeDetails.vue'

export default {
    data() {
        return {
            resumeList: [],
            drawer_resume: false,
            drawer_jobinfo: false,
            detail: {},

            // 搜索框绑定值, 搜索可以取用这里的值做匹配,好活儿
            edu_select: '',
            age_select: '',
            gender_select: '',
            matchingscore__select: '',
            name_select: '',
            jobintension_select: '',
            isLoading: false,
            svg: `
        <path class="path" d="
          M 30 15
          L 28 17
          M 25.61 25.61
          A 15 15, 0, 0, 1, 15 30
          A 15 15, 0, 1, 1, 27.99 7.5
          L 15 15
        " style="stroke-width: 4px; fill: rgba(0, 0, 0, 0)"/>
      `
        };
    },
    computed: {
        ...mapStores(useFileStore, useResumeDetail, useUser)
    },
    mounted() {
        window.JSZip = JSZip;
        // console.log("使用插件的renderAsync方法来渲染", docx);
        // console.log(this.ResumeDetailsStore)
        // const fileStore = useFileStore(pinia);

        // getResumeParser({rId: 2})
        this.getAllResumeList() // 挂载时先跑一遍, 获得以前上传过的所有信息

    },
    methods: {
        uploadFile(options) {
            // this.goPreview(options.file)
            this.isLoading = !this.isLoading
            const file = options.file
            this.filePreviewStore.uploadFile(file, this.userStore.userId, 3).then(response => {
                console.log(response)
                const detailedResume = response.detailedResume
                this.ResumeDetailsStore.Resumedetail = detailedResume

                console.log(file)
                // this.ResumeDetailsStore.set_file(file)

                // 页面间通讯, 把rid显示在url里
                const rid = detailedResume.id
                this.$router.push({
                    path: `/previewAndAlter/${rid}`
                })
            })
        },
        beforeUpload(rawfile){

        },
        // 获取以前存储的建简历信息
        getAllResumeList() {
            // 先更新state
            console.log("更新Resume state")
            console.log("userid = ", this.userStore.userId)
            this.ResumeDetailsStore.getAllResumes({ id: this.userStore.userId })
                // this.ResumeDetailsStore.getAllResumes({ userId: 1 })
                .then(() => {
                    this.resumeList = Array.from(this.ResumeDetailsStore.ResumeList)
                    console.log(this.resumeList)
                    resolve()
                })
                .catch((error) => {
                    // reject(error)
                })
        },
        // 上传成功后的回调
        // 检索
        Search() {
            // edu_select: '',
            // age_select:'',
            // gender_select: '',
            // matchingscore__select:'',
            // name_select: '',
            // jobintension_select: '',

            // 深拷贝
            this.resumeList = this.ResumeDetailsStore.ResumeList
            if (this.edu_select) {
                this.resumeList = this.resumeList.filter(item => {
                    item.highestEducation == this.edu_select
                })
            }
            if (this.age_select) {
                this.resumeList = this.resumeList.filter(item => {
                    item.age < this.age_select
                })
            }

        },
        // 开启回调
        showDetail(rid) {
            // 还是从全局事件总线调用
            getResumeParser({ id: rid })
                .then((response) => {
                    this.ResumeDetailsStore.Resumedetail = response.detailedResume
                    this.detail = this.ResumeDetailsStore.Resumedetail
                    Cookies.set('Last-detailed-resume', this.detail)
                })
                .catch(err => {
                    console.log(err)
                    reject(err)
                })
        },
        onClose() {
            this.drawer_resume = false
        }

    },
    components: { ElUpload, ElTable, ElPagination, ElInput, ElSelect, ElForm, ElButton, ElDrawer, resumeDetails, ElOption }
}
</script>