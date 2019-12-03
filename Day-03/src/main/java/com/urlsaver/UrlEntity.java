package com.urlsaver;

public class UrlEntity {
    public String partitionKey;
    public String rowKey;
    public String picUrl;
    public String username;

    public UrlEntity(String p, String r, String u, String n) {
        this.partitionKey = p;
        this.rowKey = r;
        this.picUrl = u;
        this.username = n;
    }
}