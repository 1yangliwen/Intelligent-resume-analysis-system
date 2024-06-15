<template>
    <div class="home" style="background-color: #F0F2F5; height: 750px;">
        <div style="height: 5px;" class="zhanwei">
        </div>
        <el-row :gutter="10" style="height:200px;margin-top: 10px;width: 100%;">
            <el-col :span="24">
                <ElCard style="height: 100%;margin-left: 5px;">
                    <ElForm>
                        <el-row :gutter="30" style="margin-bottom: 20px;margin-left: 10px;">

                            <el-col :span="5">
                                <ElSelect placeholder="按学历检索" v-model="edu_select" clearable="true">
                                    <ElOption v-for="item in ['专科', '本科', '硕士', '博士']" :value="item" />
                                </ElSelect>
                            </el-col>
                            <el-col :span="5">
                                <ElSelect placeholder="按年龄检索" v-model="age_select" clearable="true">
                                    <ElOption v-for="item in Array.from({ length: 100 }, (_, i) => 1 + (i))"
                                        :value="item" />
                                </ElSelect>
                            </el-col>

                            <!-- <el-col :span="5">
                                <ElSelect placeholder="按性别检索" v-model="gender_select" clearable="true">
                                    <ElOption v-for="item in ['男', '女']" :value="item" />
                                </ElSelect>
                            </el-col> -->

                            <el-col :span="5">
                                <ElSelect placeholder="选取匹配分数大于x的" v-model="matchingscore__select" clearable="true">
                                    <ElOption v-for="item in Array.from({ length: 100 }, (_, i) => 1 + (i))"
                                        :value="item" />
                                </ElSelect>
                            </el-col>

                        </el-row>
                        <el-row :gutter="20" style="margin-bottom: 20px;margin-left: 10px;">
                            <el-col :span="8">
                                <ElInput placeholder="按姓名检索" v-model="name_select">

                                </ElInput>
                            </el-col>
                            <el-col :span="8">
                                <ElInput placeholder="按求职意向检索" v-model="jobintension_select">

                                </ElInput>
                            </el-col>

                            <el-col :span="4">
                                <ElButton style="width: 100%;" @click="Search" type="primary"> 搜 索 </ElButton>
                            </el-col>
                        </el-row>
                    </ElForm>
                    <el-row :gutter="30">
                        <el-col :span="24" style="margin-left: 5px;">
                            <ElTable :data="resumeList" :border="true" style="width: 100%;" max-height="580px"
                                @cell-click="clickItem">
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

                </ElCard>

            </el-col>
        </el-row>

    </div>
</template>

<script>

import Chart from '../../components/Chart/Chart.vue';

import { useUser } from '../../stores/user';
import { useFileStore } from '../../stores/file'
import { useResumeDetail } from '../../stores/resume'

import pinia from '../../stores';
import { mapState, mapStores } from 'pinia';


import ChartAchievementsAndHighlights from '../../components/Chart/ChartAchievementsAndHighlights.vue'
import ChartCharacteristics from '../../components/Chart/ChartCharacteristics.vue'
import ChartJobMatchResult from '../../components/Chart/ChartJobMatchResult.vue'
import ChartSkillsAndExperiences from '../../components/Chart/ChartSkillsAndExperiences.vue'

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
        };
    },
    computed: {
        // ...mapState()
        ...mapStores(useFileStore, useResumeDetail, useUser)
    },
    methods: {
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
        clickItem(row, colomn, cell, event) {
            console.log(row, colomn, cell, event)
            // row是存着rid的
            this.$router.push({ path: `/talent-profile/${row.rid}`})
        },
        // Search(){
        //     this.resumeList.forEach(element => {
        //       if(element.highesteducation === )  
        //     });
        // }
    },
    mounted() {
        this.getAllResumeList() // 挂载时先跑一遍, 获得以前上传过的所有信息
    },
    components: { Chart, ChartAchievementsAndHighlights, ChartCharacteristics, ChartJobMatchResult, ChartSkillsAndExperiences }
}

</script>

<style>
.home {
    height: 700px;
}
</style>