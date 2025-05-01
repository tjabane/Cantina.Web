from fastapi import FastAPI
from dto.review import Review
from models.sentiment import analyze_sentiment

app = FastAPI()


@app.get("/api/health")
async def health():
    return {"status": "healthy"}

@app.post("/api/sentiment/")
async def sentiment_analysis(data: list[Review]):
    sentiment_results = analyze_sentiment(data)
    return sentiment_results

@app.post("/api/summary/")
async def create_summary(data: list[Review]):
    sentiment_results = analyze_sentiment(data)
    return sentiment_results