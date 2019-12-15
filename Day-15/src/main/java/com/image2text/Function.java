package com.image2text;

import java.util.*;
import com.microsoft.azure.functions.annotation.*;
import com.microsoft.azure.functions.*;
import com.microsoft.azure.cognitiveservices.vision.computervision.*;
import com.microsoft.azure.cognitiveservices.vision.computervision.models.*;

public class Function {
    @FunctionName("image2text")
    public HttpResponseMessage run(@HttpTrigger(name = "req", methods = { HttpMethod.GET,
            HttpMethod.POST }, authLevel = AuthorizationLevel.ANONYMOUS) HttpRequestMessage<Optional<String>> request,
            final ExecutionContext context) {

        String subscriptionKey = System.getenv("COMPUTER_VISION_SUBSCRIPTION_KEY");
        String endpoint = System.getenv("COMPUTER_VISION_ENDPOINT");

        ComputerVisionClient compVisClient = ComputerVisionManager.authenticate(subscriptionKey).withEndpoint(endpoint);

        Details details = AnalyzeLocalImage(compVisClient, request.getQueryParameters().get("url"));

        return request.createResponseBuilder(HttpStatus.OK).body(details).build();
    }

    public static Details AnalyzeLocalImage(ComputerVisionClient compVisClient, String url) {
        
        List<VisualFeatureTypes> featuresToExtractFromLocalImage = new ArrayList<>();
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.DESCRIPTION);
        featuresToExtractFromLocalImage.add(VisualFeatureTypes.TAGS);

        ImageAnalysis analysis = compVisClient.computerVision().analyzeImage().withUrl(url)
                .withVisualFeatures(featuresToExtractFromLocalImage).execute();

        Details details = new Details();
        for (ImageCaption caption : analysis.description().captions()) {
            details.addDescription(caption.text());
        }
        for (ImageTag tag : analysis.tags()) {
            details.addKeywords(tag.name());
        }

        return details;
    }
}