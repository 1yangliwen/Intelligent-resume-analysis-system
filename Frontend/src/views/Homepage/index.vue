<template>
    <div class="home" style="background-color: #F0F2F5; height: 1080px;">
        <div style="height: 10px;">
        </div>

        <el-row :gutter="10" style="height: 180px;">

            <el-col :span="12">
                <ElCard style="height: 95%;margin-left: 10px;">
                    <el-row :gutter="50" style="height: 50px;">
                        <el-col :span="4" :offset="4">
                            <svg-icon icon-class="documentation"
                                style="height: 50%;width: 220%; margin-top: 20px;"></svg-icon>
                        </el-col>
                        <el-col :span="10" :offset="4">
                            <div class="card-panel-text"> 简历数量</div>
                            <div class="card-panel-num">
                                {{ this.resumeNum }}
                            </div>
                        </el-col>
                        <!-- 简历数量 -->

                    </el-row>
                </ElCard>
            </el-col>

            <el-col :span="12">
                <ElCard style="height: 95%;">
                    <el-row :gutter="50" style="height: 50px;">
                        <el-col :span="4" :offset="4">
                            <svg-icon icon-class="people" style="height: 50%;width: 220%; margin-top: 20px;"></svg-icon>
                        </el-col>
                        <el-col :span="10" :offset="4">
                            <div class="card-panel-text"> 岗位数量</div>
                            <div class="card-panel-num">
                                {{ this.totalJobs }}
                            </div>
                        </el-col>
                        <!-- 简历数量 -->

                    </el-row>
                </ElCard>
            </el-col>

        </el-row>

        <el-row :gutter="10" style="height: 430px;">
            <el-col :span="24">
                <ElCard style="height: 95%;margin-left: 10px;">
                    <!-- 时序折线图 -->
                    <chart-new-add :userId="this.userStore.userId"></chart-new-add>
                </ElCard>
            </el-col>
        </el-row>

        <el-row :gutter="10" style="height: 400px;">
            <el-col :span="12">
                <ElCard style="height: 95%; margin-left: 10px;" >
                    <template #header>
                        <span style="font-weight: bold; font-size: 18px;"> 简历查看历史记录 </span>
                    </template>
                    <!-- 岗位添加历史记录 -->
                    <el-row >
                        <el-col :span="24" >
                            <ElTable :data="resumeList" :border="false" style="width: 100%;" max-height="300px" :show-header="true">
                                <el-table-column prop="name" label="姓名" />
                                <el-table-column prop="age" label="年龄" />
                                <el-table-column prop="highestEducation" label="最高学历" />
                                <el-table-column prop="jobIntention" label="求职意向" />
                            </ElTable>

                        </el-col>

                    </el-row>
                </ElCard>
            </el-col>
            <el-col :span="12">
                <ElCard style="height: 95%;">
                  <Chart :userId="this.userStore.userId"></Chart>
                </ElCard>
            </el-col>
        </el-row>

    </div>
</template>

<script>
import { ElRow, ElCol, ElUpload, ElTable, ElPagination, ElInput, ElSelect, ElForm, ElButton, ElDrawer, ElOption } from 'element-plus';
// import resumeDetails from './component/resumeDetails.vue';

import axios from 'axios';
import * as docx from 'docx-preview';
import { renderAsync } from 'docx-preview';
import JSZip from 'jszip';
import Cookies from 'js-cookie'

import { getResumeParser } from '../../api/resume'
import { getAllResumeParser } from "../../api/resume"

// 引入所有Store
import { useUser } from '../../stores/user'
import { useFileStore } from '../../stores/file'
import { useResumeDetail } from '../../stores/resume'

import pinia from '../../stores'
import { mapState, mapStores } from 'pinia'

// import { PubSub } from "pubsub-js"

// 图表
import ChartNewAdd from '../../components/Chart/ChartNewAdd.vue'
import Chart from '../../components/Chart/Chart.vue'



export default {
    data() {
        return {
            resumeList: [],
            drawer_resume: false,
            drawer_jobinfo: false,
            detail: {},
            totalJobs: 0,

            // 搜索框绑定值, 搜索可以取用这里的值做匹配,好活儿
            edu_select: '',
            age_select: '',
            gender_select: '',
            matchingscore__select: '',
            name_select: '',
            jobintension_select: '',
            isLoading: false,
            resumeNum: 0,
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
        console.log(this.userStore.roles)
        this.getAllResumeList() // 挂载时先跑一遍, 获得以前上传过的所有信息
        axios.post('http://localhost:5177/api/Home/statistics', { id: this.userStore.userId }).then((res)=>{
            console.log(res)
            this.resumeNum = res.data.totalResumes
            this.totalJobs = res.data.totalJobs
            console.log(this.resumeNum)
        })
    },
    methods: {
        // 获取以前存储的建简历信息
        getAllResumeList() {
            // 先更新state
            getAllResumeParser({id: this.userStore.userId})
                    .then((response) => {
                        // 把响应的数据更新到state里
                        console.log(response)
                        this.resumeList = response.simpleResumes
                        // console.log(this.ResumeList)
                        resolve()
                    })
                    .catch(error => {
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
            getResumeParser({ rId: rid })
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
    components: { ElUpload, ElTable, ElPagination, ElInput, ElSelect, ElForm, ElButton, ElDrawer, ElOption, ChartNewAdd, Chart }
}
</script>

<style scoped lang="scss">

.docWrap {
    width: 100%;

    .Doc {
        width: 100%;
    }
}

:deep(.docx-wrapper) {
    width: 50%;
}

:deep(.docx-wrapper > section-docx) {
    width: 100%;
}

.card-panel-text {
    padding-top: 25px;
    font-size: 24px;
    color: gray;
}

.card-panel-num {
    padding-top: 25px;
    font-size: 32px;
    font-weight: bold;
}
</style>

