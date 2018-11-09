-- Create table 附件上传表
create table UploadFiles(
  fileid         number not null primary key,
  filename       varchar2(200),
  filesize       number,
  filesuffix     varchar2(20),
  isdelete       number default 0,
  localpath      varchar2(500),
  createdate     varchar2(50) default to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
  creator        varchar2(50),
  creatorname    varchar2(50),
  deletedate     varchar2(50),
  deletingperson varchar2(50),
  deletingname   varchar2(50),
  frommodule     varchar2(50),
  orgainizid     number,
  orgainiztable  varchar2(50),
  organizname    varchar2(50) 
);

-- Add comments to the table 
comment on table UploadFiles
  is '附件上传表';

-- Add comments to the columns 
comment on column UploadFiles.fileid
  is '文件Id';
comment on column UploadFiles.filename
  is '文件名称（带扩展名）';
comment on column UploadFiles.filesize
  is '文件大小（KB）';
comment on column UploadFiles.filesuffix
  is '文件格式';
comment on column UploadFiles.isdelete
  is '指示对象是否已被删除';
comment on column UploadFiles.localpath
  is '文件所在物理路径';
comment on column UploadFiles.createdate
  is '创建时间';
comment on column UploadFiles.creator
  is '创建人';
comment on column UploadFiles.creatorname
  is '创建人姓名';
comment on column UploadFiles.deletedate
  is '删除时间';
comment on column UploadFiles.deletingperson
  is '删除人';
comment on column UploadFiles.deletingname
  is '删除人姓名';
comment on column UploadFiles.frommodule
  is '来源模块';
comment on column UploadFiles.orgainizid
  is '机构Id';
comment on column UploadFiles.orgainiztable
  is '机构所在表';
comment on column UploadFiles.organizname
  is '机构名称';