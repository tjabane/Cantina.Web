from fastapi import FastAPI
from dto.review import Review
from models.sentiment import analyze_sentiment

app = FastAPI()

@app.get("/")
async def root():
    return {"status": "alive"}


@app.post("/sentiment/")
async def sentiment_analysis(data: list[Review  | None]):
    sentiment_results = analyze_sentiment(data)
    return sentiment_results