# SCB_ClassAssignment_Frontend

This template should help get you started developing with Vue 3 in Vite.

## Recommended IDE Setup

[VSCode](https://code.visualstudio.com/) + [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur) + [TypeScript Vue Plugin (Volar)](https://marketplace.visualstudio.com/items?itemName=Vue.vscode-typescript-vue-plugin).

## Customize configuration

See [Vite Configuration Reference](https://vitejs.dev/config/).

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Compile and Minify for Production

```sh
npm run build
```

### note

选项式API得data()钩子是有this指针的, 指向当前组件实例, 结合单文件组件, 其实就是文件所代表的组件本身.

选项式API想要访问到当前组件实例指针this必须要在函数选项里

组合式API通常在script的setup钩子内调用, setup钩子被默认调用,效果如下:
- setup中声明的绑定(包括变量/函数声明/以及import导入的组件)都可以直接在template中使用
- ref在template中被访问时, 会被自动解包而不需要var.value
- 自定义组件的引入通常使用PascalCase, 干脆直接在命名的时候就用Pascal格式
- 动态组件? 让多个组件使用同一个挂载点,并可以随条件自动切换, 功能上和v-if类似, 但是会对代码的可读性产生影响
- 为了获得完整的TS类型支持, defineProps()和defineEmit是()在setup中直接可用
- 顶层await: 在setup标签下使用的await, 其异步等待会被编译为async setup()

组合式API就是一套完全声明性质的:

- 依赖注入: 
   - provide<T>(key: InjectionKey<T> | string, value: T): void
  第一个参数是要注入的key, 告诉你值的类型是什么, 方便TS类型推断
  第二个参数是要注入给后代组件的值
  > 父组件可以很方便的提供给后代的值,这个值甚至可以是十分复杂的json对象. provide-inject主要是为了解决大型的组件树问题, 当需要跨层级传递props的时候就会出现中间层组件多出很多冗余的props
   - inject<T>(key: InjectionKey<T> | string, defaultValue?: () => T | T, ?treatDefaultAsFactory: boolean)
  第一个参数是指定要接收的诸如值得名称(key), 第二个参数是是否有默认值或者默认值工厂函数; 第三个参数和默认工厂搭配使用, 给定标志位


- 生命周期钩子:


#### 响应式API

  - 响应式对象:
  ```ts
  function ref<T>(value: T): Ref<UnwrapRef<T>>  // 任意泛型
  interface Ref<T>{
      value: T 
  }

  function reactive<T extends object>(target: T): UnwrapNestedRef<T>
  ```
  reactive()一般用来处理非基本类型, 其返回值是"解包的嵌套ref"
  reactive()支持深层自动解包, 也就是所有嵌套的非基本类型对象都将被转换成响应式对象

  ref()通常用来处理基本数据类型

  - 计算属性: 
  ```ts
  computed<T>(
    option: { 
      get: () => T,
      set: (value: T) => void 
    },
    debufferOptions?: DebuggerOptions
  ): Ref<T>
  ```
   返回响应式对象, 并且计算属性又只读和可写两种, 组合式API更将计算属性变成了接近声明的形式
   返回响应式对象可以被指定是只读还是可写, 如果可写, 那么写形式和普通响应式对象一致
   计算属性的更新时间? 自动追踪响应式依赖, 检测到依赖的响应式变量变化就会同时更新所有的计算项


  计算属性与方法: 计算属性是利用的getter函数, getter函数内的所有变量会被跟踪成effect, 只有响应式依赖变化才会更新计算属性值; 方法则是每次重渲染都会更新值.

  - 侦听器: 
  ```typescript
  watch<T>(
    source: Watchsource<T>,
    callback: WatchCallback<T>,
    options?: Watchoptions
  ): StopHandle

  // 泛型类, 类别是函数
  type Watchcallback<T> = (
    value: T,   // 新值
    oldValue: T, // 旧值
    onCleanup: (cleanupFn: () => void) => void // 这一块是个什么语法结构? 
  ) => void
  ```

  watchEffect()的监听方式不需要显式地指定监听变量，而是因为source函数在vue初始化的时候执行一次，这个时候会触发回调中的所有ref变量的get，然后收集所有effect作为监听变量
  > 所以watchEffect的存在两个问题：初始化触发和多变量耦合监听

#### 组件

- 类型绑定: 绑定在组件上的class, 编译时class会被添加到组件的根元素上(所以我们推荐在template里面一定要有一个大的container)

- 父子组件通信

#### 侦听器源码解析

```ts
function watchEffect(
  effect: Watcheffect,    // 
  options?: WatchOptionsBase  // 侦听器选项基类，使用多态
): WatchStopHandle{
  return doWatch(effect, null, options)
}

// 泛型默认为any绕开ts类型检查
function watch<T = any, Immediate extends Readonly<boolean> = false>(
  source: T | WatchSource<T>,
  cb: any,
  options: WatchOptions<Immediate>
): WatchStopHandle{
  if(__DEV__ && !isFunction(cb)){ // 如果在开发环境中并且cb并不是一个回调函数，那么给出警告
    warn(...)
  }
  return doWatch(source as any, cb, options)
}
```
整体的架子就是把源码的doWatch接口包装成了两套不同的使用方式, 并返回一个停止监听的钩子（一层一层的包裹，解耦做的不错）

```ts
function doWatch(
  source: WatchSource | WatchSource[] | WatchEffect | object,
  cb: WatchCallback | null,
  { immediate, deep, flush, onTrack, onTrigger }: WatchOptions = EMPTY_OBJ,
  instance = currentInstance
): WatchStopHandle{

}
```

#### sass预处理器

在vue中使用的方式是在style标签属性中指明语言


#### webpack

- require.context()获取指定文件夹得特定文件
  参数
  - directory: (String)待引入的目录路径, 必须是相对路径
  - useSubdirectories: (Boolean)是否查询其子目录
  - regExp: (regular expression)匹配基础文件名的正则表达式

#### pinia

- 在使用useStore()钩子之前, 必须有一个已经实例化好的pinia实例, 组件内使用因为main.js一定在先执行, 因此在组件内useStore()就是开箱即用的; 但是在组件外使用【比如在request.js中拦截请求时】, 就要注意一下执行顺序(其实就是在顶层直接```const store = useStore()```时, 他是外部模块, 比vue构建要靠前, 没有实例)
- 
##### Getter

### axios

#### 响应

- data: 服务器响应的数据, 不是

### Vue router

最重要的两个接口: 
- this.$route 代表当前页面下的路由
- this.$router 代表整个router, 在这里实时更改一些配置
- 如果使用组合式API, 使用userouter()和userRoute()钩子

#### router guard 触发顺序

to be Continue