<template>
    <div class="home" style="background-color: #F0F2F5;height: 1050px;">
        <div style="height:10px">
        </div>

        <div style="margin-left: 25px;">
            <el-row>
                <el-col :span="4">
                    <h2>{{ allJob.length }} Active Job</h2>
                </el-col>
                <el-col :span="6">
                    <div style="margin-top: 20px;">
                        <span>
                            排列方式:
                        </span>
                        <el-select></el-select>
                    </div>
                </el-col>
                <el-col :span="6">
                    <div style="margin-top: 20px;">
                        <span>
                            筛选:
                        </span>
                        <el-select></el-select>
                    </div>
                </el-col>
                <el-col :span="6">
                    <div style="margin-top: 20px;">
                        <span>
                            状态:
                        </span>
                        <el-select></el-select>
                    </div>
                </el-col>
            </el-row>

        </div>
        <div style="height:10px">
        </div>

        <div style="margin-left: 20px;">
            <el-space wrap :size="12">
                <el-card v-for="item in allJob" :key="item.id" style="width: 230px;height: 300px;"
                    :class="published ? 'isPub' : 'notPub'">
                    <h3 style="margin: 0px; padding:0 0 10px 0;">{{ item.jobName }}</h3>
                    <div style="display: inline; margin-left: 0px;">
                        
                        <span style="font-size: 16px; color: #95999e; height: 20px;padding: 10px;">Tags:</span>

                        <div style="margin-left: 0px;margin-top: 5px;height: 55px;">
                            <el-row :gutter="10" >
                                <el-col  :span="24" :key="tag">
                                    <!-- <el-col v-for="tag in ['标签1','标签2','标签3','标签4']" :span="8">  -->
                                    <el-tag v-for="tag in item.jobKeywords" style="font-size: 12px; margin-bottom: 5px;margin-right: 10px;" >{{ tag }}</el-tag>
                                </el-col>
                            </el-row>

                            <!-- <el-row v-else :gutter="10">
                                <el-col v-for="tag in item.jobKeywords" :span="8">
                                <el-col v-for="tag in ['标签1', '标签2', '标签3', '标签4']" :span="8">
                                    <el-tag style="font-size: 12px; margin-bottom: 5px;" closable @close="handleClose">{{
                                        tag }}</el-tag>
                                </el-col>
                            </el-row> -->

                            <!-- <el-button class="button-new-tag ml-1" size="small">
                                <el-icon style="vertical-align: middle;">
                                    <plus />
                                    <span style="vertical-align: middle;"> 添加标签 </span>
                                </el-icon>
                            </el-button> -->
                        </div>

                    </div>



                    <div style="margin-top: 5px;">
                        <!-- <span style="font-size: 16px; color: #95999e; height: 20px;padding: 10px;">Candidates:</span> -->
                        <el-card class="candidates" shadow="never">
                            <el-row :gutter="20">
                                <el-col :span="12">
                                    <el-row>
                                        <el-col :span="4">
                                            <el-divider direction="vertical" />
                                        </el-col>
                                        <el-col :span="20">
                                            <div style="display: inline;">
                                                <span style="font-size: 16px; color: #9b9d9d;">简历总数</span>
                                                <br>
                                                <br>
                                                <span style="font-size: 24px; font-weight: bolder;">{{ item.resumeCount
                                                }}</span>
                                            </div>
                                        </el-col>

                                    </el-row>
                                </el-col>

                                <el-col :span="12">
                                    <el-row>
                                        <el-col :span="4">
                                            <el-divider direction="vertical" />
                                        </el-col>
                                        <el-col :span="20">
                                            <div style="display: inline;">
                                                <span style="font-size: 16px; color: #9b9d9d;">周简历数</span>
                                                <br>
                                                <br>
                                                <span style="font-size: 24px; font-weight: bolder;">{{ item.newResumeCount
                                                }}</span>
                                            </div>
                                        </el-col>

                                    </el-row>
                                </el-col>
                            </el-row>

                            <!-- <div style="display: inline; margin-left: 20px;"></div> -->

                        </el-card>
                    </div>

                    <div style="margin-top: 10px;">

                    </div>

                    <el-divider />

                    <div>
                        <el-row>
                            <el-col :span="14">
                                <!-- <a @click="drift">
                                <svg-icon icon-class="international"></svg-icon>
                                <span>
                                     发布
                                </span>
                                </a> -->
                            </el-col>
                            <el-col :span="10">
                                <a @click="clickItem(item.id)">
                                    <span>
                                        查看详情
                                    </span>
                                    <svg-icon icon-class="right"></svg-icon>
                                </a>
                            </el-col>
                        </el-row>
                    </div>
                </el-card>
            </el-space>
        </div>

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
            middle: true,
            published: false,

            ishover: false,
            // drifted: true,

        }
    },
    computed: {
        ...mapStores(useUser, useFileStore, useResumeDetail),
    },
    mounted() {
        axios.post('http://localhost:5177/api/Job/allJobNameOrdered', { id: this.userStore.userId }).then((res) => {
            console.log(res.data)
            this.allJob = res.data.allJobNames
            console.log(this.allJob)
            // this.$nextTick()
        }).catch((err) => {
            console.log(err)
        })

    },
    methods: {
        clickItem(jid) {
            if (this.middle === true) {
                this.middle = false
            }
            // // 岗位详细信息
            // axios.post('http://localhost:5177/api/Job/Jobinfo', { id: jid }).then((res) => {
            //     console.log(res)
            //     this.jobinfo = res.data
            // })
            // axios.post('http://localhost:5177/api/Job/Match', { userId: this.userStore.userId, jobId: jid }).then((res) => {
            //     console.log(res.data)
            //     this.matchResume = res.data.matches
            //     this.showJobDetais = true
            // })
            // 跳转
            this.$router.push({
                path: `/job-matching/${jid}`
            })
        },
        clickResume(row, column, event) {
            console.log(row, column, event)
            this.$router.push({ path: `/talent-profile/${row.resumeId}` })
        },
        handleClose(tag, item) {
            item.jobKeywords.value.splice(item.jobKeywords.value.indexOf(tag), 1)
        },
        publish(){

        },
        drift(){

        }
    },
    components: { ElTable }
}
</script>


<style lang="scss" scoped>
.candidates {
    background-color: #e9eaeb;
    margin: 0px;
    height: 120px;

    .inside {
        display: inline;
        margin-left: 10px;
        height: 80px;
    }
}

:deep(.el-card__body) {
    padding: 10px;
}

:deep(.el-divider--vertical) {
    display: inline-block;
    width: 1px;
    height: 100px;
    margin: 0 4px;
    vertical-align: middle;
    position: relative;
    border-left: 3px rgb(90, 176, 103) var(--el-border-style);
}

:deep(.el-divider--horizontal) {
    display: block;
    height: 1px;
    width: 100%;
    margin: 0 0 10px 0;
    border-top: 1px var(--el-border-color) var(--el-border-style);
}

:deep(.svg-icon) {
    vertical-align: 0em;
    height: 0.8em;
}

.isPub {
    border-top: solid green 5px;
}

.notPub{
    border-top: solid gray 5px;
}
</style>