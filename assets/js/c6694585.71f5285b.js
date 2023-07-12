"use strict";(self.webpackChunkthreads_api_documentation=self.webpackChunkthreads_api_documentation||[]).push([[827],{3905:(e,t,r)=>{r.d(t,{Zo:()=>c,kt:()=>f});var n=r(7294);function a(e,t,r){return t in e?Object.defineProperty(e,t,{value:r,enumerable:!0,configurable:!0,writable:!0}):e[t]=r,e}function o(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var n=Object.getOwnPropertySymbols(e);t&&(n=n.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,n)}return r}function l(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?o(Object(r),!0).forEach((function(t){a(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):o(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}function s(e,t){if(null==e)return{};var r,n,a=function(e,t){if(null==e)return{};var r,n,a={},o=Object.keys(e);for(n=0;n<o.length;n++)r=o[n],t.indexOf(r)>=0||(a[r]=e[r]);return a}(e,t);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(e);for(n=0;n<o.length;n++)r=o[n],t.indexOf(r)>=0||Object.prototype.propertyIsEnumerable.call(e,r)&&(a[r]=e[r])}return a}var i=n.createContext({}),p=function(e){var t=n.useContext(i),r=t;return e&&(r="function"==typeof e?e(t):l(l({},t),e)),r},c=function(e){var t=p(e.components);return n.createElement(i.Provider,{value:t},e.children)},u="mdxType",m={inlineCode:"code",wrapper:function(e){var t=e.children;return n.createElement(n.Fragment,{},t)}},d=n.forwardRef((function(e,t){var r=e.components,a=e.mdxType,o=e.originalType,i=e.parentName,c=s(e,["components","mdxType","originalType","parentName"]),u=p(r),d=a,f=u["".concat(i,".").concat(d)]||u[d]||m[d]||o;return r?n.createElement(f,l(l({ref:t},c),{},{components:r})):n.createElement(f,l({ref:t},c))}));function f(e,t){var r=arguments,a=t&&t.mdxType;if("string"==typeof e||a){var o=r.length,l=new Array(o);l[0]=d;var s={};for(var i in t)hasOwnProperty.call(t,i)&&(s[i]=t[i]);s.originalType=e,s[u]="string"==typeof e?e:a,l[1]=s;for(var p=2;p<o;p++)l[p]=r[p];return n.createElement.apply(null,l)}return n.createElement.apply(null,r)}d.displayName="MDXCreateElement"},7027:(e,t,r)=>{r.r(t),r.d(t,{assets:()=>i,contentTitle:()=>l,default:()=>m,frontMatter:()=>o,metadata:()=>s,toc:()=>p});var n=r(7462),a=(r(7294),r(3905));const o={sidebar_label:"Examples",sidebar_position:2},l="Examples",s={unversionedId:"examples/examples",id:"examples/examples",title:"Examples",description:"Post",source:"@site/docs/examples/examples.md",sourceDirName:"examples",slug:"/examples/",permalink:"/examples/",draft:!1,editUrl:"https://github.com/facebook/docusaurus/tree/main/packages/create-docusaurus/templates/shared/docs/examples/examples.md",tags:[],version:"current",sidebarPosition:2,frontMatter:{sidebar_label:"Examples",sidebar_position:2},sidebar:"tutorialSidebar",previous:{title:"Intro",permalink:"/"},next:{title:"Tutorial - Basics",permalink:"/category/tutorial---basics"}},i={},p=[{value:"Post",id:"post",level:3},{value:"Follow &amp; UnFollow",id:"follow--unfollow",level:3}],c={toc:p},u="wrapper";function m(e){let{components:t,...r}=e;return(0,a.kt)(u,(0,n.Z)({},c,r,{components:t,mdxType:"MDXLayout"}),(0,a.kt)("h1",{id:"examples"},"Examples"),(0,a.kt)("h3",{id:"post"},"Post"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-csharp"},'IThreadsApi api = new ThreadsApi(new HttpClient());\nvar authToken = await api.GetTokenAsync("tidusjar", "password");\nawait api.PostAsync("tidusjar", "Hello!", authToken);\n')),(0,a.kt)("h3",{id:"follow--unfollow"},"Follow & UnFollow"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-csharp"},'IThreadsApi api = new ThreadsApi(new HttpClient());\n\nvar authToken = await api.GetTokenAsync("tidusjar", "password");\n\nvar userNameToFollow = "zuck";\nvar userToFollow = await api.GetUserIdFromUserNameAsync(userNameToFollow);\nawait api.FollowAsync(userToFollow.UserId, userToFollow.Token, authToken);\nawait api.UnFollowAsync(userToFollow.UserId, userToFollow.Token, authToken);\n')))}m.isMDXComponent=!0}}]);