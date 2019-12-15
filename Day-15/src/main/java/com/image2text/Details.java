package com.image2text;

import java.util.*;

public class Details {
    public List<String> Descriptions;
    public List<String> Keywords;

    public Details() {
        Descriptions = new ArrayList<String>();
        Keywords = new ArrayList<String>();
    }

    public void addDescription(String description) {
        Descriptions.add(description);
    }

    public void addKeywords(String keyword) {
        Keywords.add(keyword);
    }
}
