<template>
    <div>

    <div style="height: 5px;" class="zhanwei">
    </div>
    <ElRow :gutter="10">
        <ElCol :span="11" style="margin-left: 5px;">
            <!-- <div class="docWrap">
                <div ref="file" class="Doc" style="width: 100%;"></div>
            </div> -->
            <ElCard>
                <getResumeFromBackend :RId="this.$route.params.id"></getResumeFromBackend>
            </ElCard>

        </ElCol>
        <ElCol :span="12">
            <el-scrollbar max-height="780px">

                <div class="card-warpper">
                    <ElForm :model="resumeDetails">
                        <!-- <ElFormItem required> -->
                            <ElCard :header="titles[0]" style="margin-bottom: 30px;">
                                <!-- 基本信息 -->
                                <span>姓名: </span>
                                <ElInput v-model="resumeDetails.name" margin-bottom="10px"></ElInput>
                                <span>年龄: </span>
                                <ElInput v-model="resumeDetails.age" margin-bottom="10px"></ElInput>
                                <span>性别: </span>
                                <ElInput v-model="resumeDetails.gender" margin-bottom="10px"></ElInput>
                                <span>电子邮箱: </span>
                                <ElInput v-model="resumeDetails.email" margin-bottom="10px"></ElInput>
                                <span>联系电话: </span>
                                <ElInput v-model="resumeDetails.phoneNumber" margin-bottom="10px"></ElInput>
                                <span>最高学历: </span>
                                <ElInput v-model="resumeDetails.highestEducation" margin-bottom="10px"></ElInput>
                            </ElCard>
                        <!-- </ElFormItem> -->


                        <ElCard :header="titles[1]" style="margin-bottom: 30px;">
                            <!-- 工作经历 -->
                            <template v-for="(item, index) in resumeDetails.workExperience" :key="index">
                                <span>公司: </span>
                                <ElInput v-model="item.companyName" margin-bottom="10px"></ElInput>
                                <span>职位: </span>
                                <ElInput v-model="item.position" margin-bottom="10px"></ElInput>
                                <span>时间: </span>
                                <ElInput v-model="item.time" margin-bottom="10px"></ElInput>
                                <span>具体经历: </span>
                                <ElInput v-model="item.task" margin-bottom="10px"></ElInput>
                            </template>

                        </ElCard>

                        <ElCard :header="titles[2]" style="margin-bottom: 30px;">
                            <!-- 教育背景 -->
                            <template v-for="(item, index) in resumeDetails.educationBackgrounds" :key="index">
                                <span>时间: </span>
                                <ElInput v-model="item.time"></ElInput>
                                <span>学校: </span>
                                <ElInput v-model="item.school"></ElInput>
                                <template v-if="item.major !== null">
                                    <span>专业: </span>
                                    <ElInput v-model="item.task"></ElInput>
                                </template>
                            </template>
                        </ElCard>

                        <ElCard :header="titles[3]" style="margin-bottom: 30px;">
                            <!-- 获奖情况 -->
                            <ElInput v-for="(item, index) in resumeDetails.awards" :key="index" v-model="item.awardName">
                            </ElInput>
                        </ElCard>

                        <ElCard :header="titles[4]" style="margin-bottom: 30px;">
                            <!-- 技能 -->
                            <ElInput v-for="(item, index) in resumeDetails.skillCertificate" :key="index"
                                v-model="item.skillName"></ElInput>
                        </ElCard>

                        <ElCard :header="titles[6]" style="margin-bottom: 30px;">
                            <!-- 自我评价 -->
                            <ElInput v-model="resumeDetails.selfEvaluation"></ElInput>
                        </ElCard>


                        <ElCard :header="titles[7]" style="margin-bottom: 30px;">
                            <!-- 模型评价 -->
                            <span>工作稳定性: </span>
                            <ElInput v-model="resumeDetails.workStability"></ElInput>
                            <span>原因: </span>
                            <ElInput v-model="resumeDetails.workStabilityReason"></ElInput>
                        </ElCard>

                        <ElFormItem>
                            <ElButton type="primary" @click="onSubmit"> 保存修改 </ElButton>
                            <ElButton> 取消 </ElButton>
                        </ElFormItem>

                    </ElForm>

                </div>

            </el-scrollbar>
        </ElCol>

    </ElRow>
            
</div>
</template>
<script>
import { ElButton, ElCard, ElCol, ElForm, ElFormItem, ElInput, ElRow } from 'element-plus';
import { renderAsync } from 'docx-preview';
import JSZip from 'jszip';
import { useResumeDetail } from '../../stores/resume';
import { mapStores } from 'pinia';
import getResumeFromBackend from '../../components/Chart/getResumeFromBackend.vue'
import axios from 'axios';

window.JSZip = JSZip;

export default {
    data() {
        return {
            titles: ['基本信息', '工作经历', '教育背景', '获奖情况', '技能', '求职信息', '自我评价', 'AI分析'],
            resumeDetails: {}
        }
    },
    mounted() {
        this.resumeDetails = this.ResumeDetailsStore.Resumedetail
        console.log(this.resumeDetails)
        // this.goPreview(this.ResumeDetailsStore.file);
    },
    computed: {
        ...mapStores(useResumeDetail)
    },
    methods: {
        // 预览
        goPreview(data) {
            console.log(data);
            renderAsync(data, this.$refs.file, null, {
                className: "text-docx",
                inWrapper: false,
                ignoreWidth: false,
                ignoreHeight: false,
                ignoreFonts: false,
                breakPages: true,
                ignoreLastRenderedPageBreak: true,
                experimental: true,
                trimXmlDeclaration: true,
                useBase64URL: false,
                useMathMLPolyfill: true,
                debug: false //enables additional logging
            }); // 渲染到页面
            // this.cardDisplay = true;
        },
        alterDetails() {

        },
        onSubmit() {
            let RS = this.resumeDetails
            for (const key in RS) {
                if (RS[key] === null) {
                    RS[key] = '-'
                }
            }
            axios.post('http://localhost:5177/api/Resume/SecondUpload', { code: 0, detailedResume: RS }).then((res) => {
                console.log(res)
                if (res.status === 200) {
                    this.$router.push({ path: '/uploadResume' })
                }
            })

        }

    },
    components: { ElCol, ElCard, ElForm, ElInput, ElButton, ElFormItem, getResumeFromBackend }
}

</script>

