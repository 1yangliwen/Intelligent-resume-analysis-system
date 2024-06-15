<template>
    <div class="home" style="background-color: #F0F2F5; height: 1080px;">
        <div style="height: 10px;" class="zhanwei">
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
                                {{ resumeNum }}
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
                                {{  }}
                            </div>
                        </el-col>
                        <!-- 简历数量 -->

                    </el-row>
                </ElCard>
            </el-col>

        </el-row>


        <el-row :gutter="10" style="height:400px">
            <el-col :span="8">
                <ElCard style="height: 90%;margin-left: 5px;">
                    <!-- 应聘人年龄段 -->
                    <ChartAgeGroups :userId="this.userStore.userId"></ChartAgeGroups>
                </ElCard>
            </el-col>
            <el-col :span="8">
                <ElCard style="height: 90%;">
                                        <!-- 岗位信息统计 -->
                                        <Chart :userId="this.userStore.userId"></Chart>

                </ElCard>
            </el-col>
            <el-col :span="8">
                <ElCard style="height: 90%;">
                    <!-- 工作稳定性 -->
                    <ChartWorkStability :userId="this.userStore.userId"></ChartWorkStability>
                </ElCard>
            </el-col>
        </el-row>

        <el-row :gutter="10" style="height:500px">
            <el-col :span="12">
                <ElCard style="height: 100%;margin-left: 5px;">
                    <!-- 人岗匹配程度 -->
                    <ChartMatch :userId="this.userStore.userId"></ChartMatch>
                </ElCard>
            </el-col>
            <el-col :span="12">
                <ElCard style="height: 100%;margin-left: 5px;">
                    <!-- 最高学历以及毕业院校 -->
                    <ChartEducation :userId="this.userStore.userId"></ChartEducation>
                </ElCard>
            </el-col>
        </el-row>

        <!-- <el-row :gutter="10" style="height:1000px;margin-top: 10px;">
            <el-col :span="24">
                <ElCard style="height: 100%;margin-left: 5px;">
                    岗位数量
                </ElCard>

            </el-col>
        </el-row> -->

    </div>
</template>

<script>
import { useUser } from '../../stores/user';
import { useFileStore } from '../../stores/file'
import { useResumeDetail } from '../../stores/resume'

import pinia from '../../stores';
import { mapState, mapStores } from 'pinia';
import axios from 'axios'

// 图表
import ChartAgeGroups from '../../components/Chart/ChartAgeGroups.vue'
import ChartEducation from '../../components/Chart/ChartEducation.vue'
import ChartWorkStability from '../../components/Chart/ChartWorkStability.vue'
import ChartMatch from '../../components/Chart/ChartMatchScores.vue'
import Chart from '../../components/Chart/Chart.vue'


export default {
    data() {
        return {
            resumeNum: 0,
            totalJobs: 0
        };
    },
    computed: {
        // ...mapState()
        ...mapStores(useFileStore, useResumeDetail, useUser)
    },
    methods: {},
    mounted() {
        axios.post('http://localhost:5177/api/Home/statistics', { id: this.userStore.userId }).then((res)=>{
            console.log(res)
            this.resumeNum = res.data.totalResumes
            this.totalJobs = res.data.totalJobs
            console.log(this.resumeNum)
        })
    },
    components: { ChartAgeGroups, ChartEducation, ChartWorkStability, ChartMatch, Chart }
}

</script>

<style>
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