<template>
    <div class="home" style="background-color: #F0F2F5;height: 800px;">
        <div style="height:10px">
        </div>
        <el-row :gutter="10">
            <el-col :span="10">
                <el-row style="height: 400px;">
                    <el-col>
                        <ElCard style="margin-left: 5px;">
                            <el-table :data="this.allJob" style="width: 100%;" @cell-click="clickItem" max-height="350px">
                                <el-table-column prop="id" label="序号" />
                                <el-table-column prop="jobName" label="岗位名称" />
                            </el-table>
                        </ElCard>
                    </el-col>
                </el-row>
                <el-row style="height: 400px;">
                    <el-col>
                        <ElCard style="margin-left: 5px;height: 90%;" header="岗位详细信息">
                            <template v-if="showJobDetais">
                                <el-row style="margin-bottom: 10px">
                                    <span>
                                        名称
                                    </span>
                                    <el-text>
                                        {{ jobinfo.jobName }}
                                    </el-text>
                                </el-row>

                                <el-row style="margin-bottom: 10px">
                                    <span>
                                        要求
                                    </span>
                                    <el-text>
                                        {{ jobinfo.jobDetails }}
                                    </el-text>
                                </el-row>

                            </template>

                            <el-text>

                            </el-text>
                        </ElCard>
                    </el-col>
                </el-row>
            </el-col>

            <el-col :span="14">
                <!-- <ElCard>
                    <ElTable :data="this.matchResume" max-height="700px" @row-click="clickResume">
                        <el-table-column prop="name" label="姓名" />
                        <el-table-column prop="score" label="匹配分数" />
                        <el-table-column prop="matchReason" label="匹配原因" />
                    </ElTable>

                </ElCard> -->

                <el-scrollbar height="800px">
                    <ElCard v-for="item in matchResume" :key="matchResume">
                        {{ item.matchReason }}
                    </ElCard>
                </el-scrollbar>

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
            jobinfo: null
        }
    },
    computed: {
        ...mapStores(useUser, useFileStore, useResumeDetail)
    },
    mounted() {
        axios.post('http://localhost:5177/api/Job/allJobName', { id: this.userStore.userId }).then((res) => {
            console.log(res.data)
            this.allJob = res.data.allJobNames
            console.log(this.allJob)
            // this.$nextTick()
        }).catch((err) => {
            console.log(err)
        })
    },
    methods: {
        clickItem(row, column, event) {
            console.log(row, column, event)
            // row是存着rid的
            console.log(row.id)
            axios.post('http://localhost:5177/api/Job/Match', { userId: this.userStore.userId, jobId: row.id }).then((res) => {
                console.log(res.data)
                this.matchResume = res.data.matches
            })

            axios.post('http://localhost:5177/api/Job/Jobinfo', { id: row.id }).then((res) => {
                console.log(res)
                this.jobinfo = res.data
                this.showJobDetais = true
            })
        },
        clickResume(row, column, event) {
            console.log(row, column, event)
            this.$router.push({ path: `/talent-profile/${row.resumeId}` })
        }
    },
    components: { ElTable }
}
</script>

