import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.neighbors import KNeighborsClassifier
from sklearn.metrics import classification_report, confusion_matrix
import matplotlib.pyplot as plt
import seaborn as sns

# قراءة البيانات
try:
    dataset = pd.read_csv('train.csv')
except FileNotFoundError:
    print("ملف البيانات غير موجود. يرجى التأكد من وجود الملف في المسار الصحيح.")
    exit()

# تحضير البيانات
X = dataset.drop('price_range', axis=1)
y = dataset['price_range']

# تقسيم البيانات
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.33, random_state=101)

# إنشاء وتدريب نموذج KNN
knn = KNeighborsClassifier(n_neighbors=10)
knn.fit(X_train, y_train)

# تقييم النموذج
print("دقة النموذج:", knn.score(X_test, y_test))

# التنبؤ
predictions = knn.predict(X_test)
print("\nتقرير التصنيف:")
print(classification_report(y_test, predictions))

# مصفوفة الارتباك
matrix = confusion_matrix(y_test, predictions)
plt.figure(figsize=(10,7))
sns.heatmap(matrix, annot=True)
plt.title('مصفوفة الارتباك')
plt.show()

# حفظ النموذج
import joblib
joblib.dump(knn, 'mobile_price_model.pkl') 