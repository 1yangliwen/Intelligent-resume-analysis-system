<template>
    <div>

        <ElForm>
            <el-row :gutter="20" style="margin-bottom: 20px;">

                <el-col :span="5" :offset="2">
                    <ElSelect placeholder="按学历检索" v-model="edu_select" clearable="true">
                        <ElOption v-for="item in ['专科', '本科', '硕士', '博士']" :value="item" />
                    </ElSelect>
                </el-col>
                <el-col :span="5">
                    <ElSelect placeholder="按年龄检索" v-model="age_select" clearable="true">
                        <ElOption v-for="item in Array.from({ length: 100 }, (_, i) => 1 + (i))" :value="item" />
                    </ElSelect>
                </el-col>

                <el-col :span="5" :offset="1">
                    <ElSelect placeholder="按性别检索" v-model="gender_select" clearable="true">
                        <ElOption v-for="item in ['男', '女']" :value="item" />
                    </ElSelect>
                </el-col>

                <el-col :span="5">
                    <ElSelect placeholder="选取匹配分数大于x的" v-model="matchingscore__select" clearable="true">
                        <ElOption v-for="item in Array.from({ length: 100 }, (_, i) => 1 + (i))" :value="item" />
                    </ElSelect>
                </el-col>

            </el-row>
            <el-row :gutter="20" style="margin-bottom: 20px;">
                <el-col :span="8" :offset="2">
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
        
    </div>
</template>

<script>
import { ElRow, ElCol, ElUpload, ElTable, ElPagination, ElInput, ElSelect, ElForm, ElButton, ElDrawer, ElOption } from 'element-plus';

</script>