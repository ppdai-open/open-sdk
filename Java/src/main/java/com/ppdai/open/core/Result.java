package com.ppdai.open.core;

/**
 * Created by xuzhishen on 2016/3/16.
 */
public class Result {
    /**
     * 是否成功
     */
    private boolean sucess;

    /**
     * 报文内容
     */
    private String context;

    /**
     * 错误信息
     */
    private String errorMessage ;

    public boolean isSucess() {
        return sucess;
    }

    public void setSucess(boolean sucess) {
        this.sucess = sucess;
    }

    public String getContext() {
        return context;
    }

    public void setContext(String context) {
        this.context = context;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    public void setErrorMessage(String errorMessage) {
        this.errorMessage = errorMessage;
    }
}
