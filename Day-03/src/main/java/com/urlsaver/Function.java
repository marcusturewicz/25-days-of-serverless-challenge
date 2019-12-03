package com.urlsaver;

import java.io.IOException;
import java.util.*;
import com.microsoft.azure.functions.annotation.*;
import com.microsoft.azure.functions.*;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;

public class Function {
     private static ObjectMapper mapper = new ObjectMapper();
    @FunctionName("urlsaver")
    public HttpResponseMessage run(
            @HttpTrigger(name = "req", methods = {HttpMethod.GET, HttpMethod.POST}, authLevel = AuthorizationLevel.FUNCTION) HttpRequestMessage<Optional<String>> request,
            @TableOutput(name = "outputTable", tableName = "Url", connection = "AzureWebJobsStorage") OutputBinding<UrlEntity> outputData,
            final ExecutionContext context) {
        context.getLogger().info("Java HTTP trigger processed a request.");

        String json = request.getBody().get();
        
        try {
            
            JsonNode jsonNode = mapper.readTree(json);
            
            String branch = jsonNode.get("ref").asText().split("/")[2];
            String htmlUrl = jsonNode.get("repository").get("html_url").asText();
            String username = jsonNode.get("head_commit").get("committer").get("username").asText();
            JsonNode added = jsonNode.get("head_commit").get("added");

            if (added.isArray()) {
                for (final JsonNode value : added) {
                    String filename = value.asText();
                    if (filename.endsWith(".png"))
                    {
                        // Construct url
                        String url = String.join("/", htmlUrl, "blob", branch, filename);

                        // Row key must be unique and not contain "/"
                        String rowKey = username + url.split("https://github.com")[1].replace("/", "-");

                        // Save data in table storage
                        outputData.setValue(new UrlEntity("partition", rowKey, url, username));
                    }
                }
            }

        } catch (IOException e) {
            e.printStackTrace();
        }

        return request.createResponseBuilder(HttpStatus.OK).body("Url Saved").build();
    }
}
