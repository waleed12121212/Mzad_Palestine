import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestRegressor
from sklearn.preprocessing import StandardScaler
import joblib
import os
import logging

# إعداد السجلات
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# إنشاء بيانات تدريب وهمية
def create_sample_data():
    logger.info("جاري إنشاء بيانات التدريب...")
    np.random.seed(42)
    n_samples = 1000
    
    data = {
        'battery_capacity': np.random.randint(3000, 6000, n_samples),
        'display_size': np.random.uniform(5.0, 7.0, n_samples),
        'storage': np.random.choice([64, 128, 256, 512, 1024], n_samples),
        'ram': np.random.choice([4, 6, 8, 12, 16], n_samples),
        'refresh_rate': np.random.choice([60, 90, 120, 144], n_samples),
        'front_camera_mp': np.random.choice([8, 12, 16, 20, 32], n_samples),
        'rear_camera_mp': np.random.choice([12, 48, 64, 108, 200], n_samples),
        'charging_speed': np.random.choice([15, 25, 33, 45, 65, 100], n_samples)
    }
    
    # حساب السعر بناءً على المواصفات
    base_price = 1000
    price = (
        data['battery_capacity'] * 0.1 +
        data['display_size'] * 100 +
        data['storage'] * 0.5 +
        data['ram'] * 50 +
        data['refresh_rate'] * 2 +
        data['front_camera_mp'] * 10 +
        data['rear_camera_mp'] * 5 +
        data['charging_speed'] * 3 +
        base_price
    )
    
    data['price'] = price
    logger.info("تم إنشاء بيانات التدريب بنجاح")
    return pd.DataFrame(data)

# إنشاء وتدريب النموذج
def train_model():
    try:
        logger.info("بدء تدريب النموذج...")
        # إنشاء بيانات التدريب
        df = create_sample_data()
        
        # تحضير البيانات
        X = df.drop('price', axis=1)
        y = df['price']
        
        # تقسيم البيانات
        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
        
        # تحجيم البيانات
        scaler = StandardScaler()
        X_train_scaled = scaler.fit_transform(X_train)
        X_test_scaled = scaler.transform(X_test)
        
        # إنشاء وتدريب النموذج
        model = RandomForestRegressor(n_estimators=100, random_state=42)
        model.fit(X_train_scaled, y_train)
        
        # حفظ النموذج والمحول
        model_path = os.path.join(os.path.dirname(__file__), '..', 'phone_price_model.pkl')
        scaler_path = os.path.join(os.path.dirname(__file__), '..', 'phone_price_scaler.pkl')
        
        joblib.dump(model, model_path)
        joblib.dump(scaler, scaler_path)
        
        logger.info(f"تم حفظ النموذج في: {model_path}")
        logger.info(f"تم حفظ المحول في: {scaler_path}")
        
        return model, scaler
    except Exception as e:
        logger.error(f"حدث خطأ أثناء تدريب النموذج: {str(e)}")
        raise

# التنبؤ بالسعر
def predict_price(phone_specs):
    try:
        logger.info(f"جاري التنبؤ بالسعر للمواصفات: {phone_specs}")
        
        # تحميل النموذج والمحول
        model_path = os.path.join(os.path.dirname(__file__), '..', 'phone_price_model.pkl')
        scaler_path = os.path.join(os.path.dirname(__file__), '..', 'phone_price_scaler.pkl')
        
        if not os.path.exists(model_path) or not os.path.exists(scaler_path):
            logger.error("ملفات النموذج غير موجودة")
            raise FileNotFoundError("ملفات النموذج غير موجودة")
        
        model = joblib.load(model_path)
        scaler = joblib.load(scaler_path)
        
        # تحويل المواصفات إلى DataFrame
        specs_df = pd.DataFrame([phone_specs])
        
        # تحجيم البيانات
        specs_scaled = scaler.transform(specs_df)
        
        # التنبؤ بالسعر
        predicted_price = model.predict(specs_scaled)[0]
        
        logger.info(f"السعر المتوقع: {predicted_price}")
        return round(predicted_price, 2)
    except Exception as e:
        logger.error(f"حدث خطأ أثناء التنبؤ بالسعر: {str(e)}")
        raise

if __name__ == "__main__":
    # تدريب النموذج
    train_model() 