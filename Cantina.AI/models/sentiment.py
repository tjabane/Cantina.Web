from transformers import pipeline
import numpy as np

def analyze_sentiment(data):
    reviews = [review.__dict__ for review in data]
    sentiment_pipeline = pipeline("text-classification", model="tabularisai/multilingual-sentiment-analysis")
    sentiment_results = sentiment_pipeline(review["Comment"] for review in reviews)
    joined_results = [{"Id": review["Id"]  ,"Comment": review["Comment"], "Sentiment": result["label"]} for review, result in zip(reviews, sentiment_results)]
    return joined_results