<template>
    <div class="home" style="background-color: #F0F2F5;height: 800px;">
        <div style="height:10px">
        </div>
        <el-row :gutter="10">
            <el-col :span="12">
                <el-row style="height: 400px;">
                    <el-col>
                        <el-scrollbar height="750px">
                            <el-card style="margin-left: 5px;">
                                <a @click="getBack">
                                    <svg-icon icon-class="left"></svg-icon>
                                    <span style="font-size: 18px;"> 返回</span>
                                </a>
                                <el-collapse style="margin-left: 5px; margin-top: 5px;"
                                    :model-value="this.$route.params.jid" accordion>
                                    <div v-for="item in allJob" :key="item.id" style="margin-left: 5px;"
                                        @click="clickItem(item.id)">
                                        <el-collapse-item :title="item.jobName" :name="item.id" :id="'k' + item.id">
                                            <!-- 折叠框子项并不能绑定点击事件 -->
                                            <template v-if="showJobDetais">
                                                <el-row style="margin-bottom: 10px">
                                                    <el-text>
                                                        {{ jobinfo.jobDetails }}
                                                    </el-text>
                                                </el-row>

                                            </template>
                                        </el-collapse-item>
                                    </div>
                                </el-collapse>

                            </el-card>
                        </el-scrollbar>
                    </el-col>
                </el-row>
            </el-col>

            <el-col :span="12">
                <el-scrollbar height="750px">
                    <ElCard v-for="item in matchResume" :key="item.resumeId" style="margin-bottom: 10px;">
                        <!-- {{ item.matchReason }} -->
                        <h3 style="margin-top: 0; margin-bottom: 0;">
                            {{ item.name }} | {{ item.graduatedFrom }} | {{ item.totalWorkYears }}年工作经验
                        </h3>
                        <!-- <el-rate v-model="item.score" disabled allow-half show-score text-color="#ff9900"
                            score-template="{value} points" /> -->
                        <div style="margin-left: 10px;">
                            <!-- <span style="font-size:14px; font-weight:bold">人岗匹配分数: </span> -->
                            <el-rate v-model="exampleScore" disabled allow-half show-score text-color="#ff9900"
                                score-template="{value}" />
                        </div>


                        <ul style="margin:5px 0 0 0;padding-left: 20px;">
                            <li style="height: 30px; ">
                                <el-row>
                                    <el-col :span="4">
                                        <span style="font-weight: bold;">学 历:</span>
                                    </el-col>
                                    <el-col :span="20">
                                        <span>{{ item.highestEducation }}</span>
                                    </el-col>
                                </el-row>
                            </li>
                            <li style="height: 30px; ">
                                <el-row>
                                    <el-col :span="4">
                                        <span style="font-weight: bold;">专 业:</span>
                                    </el-col>
                                    <el-col :span="20">
                                        <span>{{ item.major }}</span>
                                    </el-col>
                                </el-row>
                            </li>
                        </ul>
                        <div>
                            <!-- <el-tag v-for="sc in Array.from({ length: 7 }, (_, i) => 1 + (i))" class="ml-2" type="success" style="margin-right: 5px;margin-top: 10px">标签示例(长度可变)</el-tag> -->
                            <el-tag v-for="sc in item.workTraits" class="ml-2" type="success" style="margin-right: 5px;margin-top: 10px">{{ sc }}</el-tag>
                        </div>
                    </ElCard>
                </el-scrollbar>

                <!-- "matches": [
    {
      "resumeId": 0,
      "name": "string",
      "score": 0,
      "highestEducation": "string",
      "totalWorkYears": 0,
      "graduatedFrom": "string",
      "major": "string",
      "workTraits": [
        "string"
      ],
      "matchReason": "string"
    }
  ]
} -->
            </el-col>
        </el-row>
    </div>
</template>

<script>
import { ElRow, ElTable } from 'element-plus';
import axios from 'axios'

import { useUser } from '../../stores/user';
import { useFileStore } from '../../stores/file'
import { useResumeDetail } from '../../stores/resume'

import pinia from '../../stores';
import { mapState, mapStores } from 'pinia';


export default {
    data() {
        return {
            allJob: [],
            matchResume: [],
            showJobDetais: false,
            jobinfo: null,
            activeItem: '',

            exampleScore: 5
        }
    },
    computed: {
        ...mapStores(useUser, useFileStore, useResumeDetail),
        scoreFixed(score) {
            return 0
        }
    },
    mounted() {
        axios.post('http://localhost:5177/api/Job/allJobNameOrdered', { id: this.userStore.userId }).then((res) => {
            this.allJob = res.data.allJobNames
            this.activeItem = this.$route.params.jid
            console.log(this.$route.params.jid)

            this.$nextTick(() => {
                console.log(document.getElementById('k' + this.$route.params.jid).childNodes[0].childNodes[0])
                document.getElementById('k' + this.$route.params.jid).childNodes[0].childNodes[0].click()
                // this.clickItem(parseInt(this.activeItem))
            })
        }).catch((err) => {
            console.log(err)
        })

    },
    methods: {
        clickItem(jid) {
            // 岗位详细信息
            axios.post('http://localhost:5177/api/Job/Jobinfo', { id: jid }).then((res) => {
                console.log(res)
                this.jobinfo = res.data
            })
            axios.post('http://localhost:5177/api/Job/Match', { userId: this.userStore.userId, jobId: jid }).then((res) => {
                console.log(res.data)
                this.matchResume = res.data.matches
                this.showJobDetais = true
            })
        },
        clickResume(row, column, event) {
            console.log(row, column, event)
            this.$router.push({ path: `/talent-profile/${row.resumeId}` })
        },
        getBack() {
            this.$router.push({ path: '/job-info' })
        }
    },
    components: { ElTable }
}
</script>

<style scoped>
:deep(.el-collapse-item__header) {
    font-size: 16px;
    font-weight: bold;
}
</style>