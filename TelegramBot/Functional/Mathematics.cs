using System;
using System.Collections.Generic;
using System.IO;

namespace TelegramBot.Functional {
    public class Mathematics {
        private class Determinant {
            private readonly double[,] mas = null;
            private int mas_Length;
            private string mas_str = "";

            private bool Check_Error = false;

            public Determinant(string[] elements) {
                double root = Math.Round(Math.Sqrt(elements.Length), 0);
                if (root * root == elements.Length)
                    mas_Length = Convert.ToInt32(Math.Round(Math.Sqrt(elements.Length), 0));
                else
                    Check_Error = true;

                if (!Check_Error) {
                    mas = new double[mas_Length, mas_Length];
                    for (int i = 0, ind = 0; i < mas_Length; i++) {
                        mas_str += "| ";
                        for (int j = 0; j < mas_Length; j++, ind++) {
                            if (Double.TryParse(elements[ind], out mas[i, j])) {
                                mas_str += elements[ind] + " ";
                            }
                            else {
                                mas_str += "! ";
                                Check_Error = true;
                            }
                        }
                        mas_str += "|\n";
                    }
                }
                 

            }
            public Determinant(double[,] elements) {
                mas_Length = elements.GetLength(0);
                mas = new double[mas_Length, mas_Length];

                for (int i = 0; i < mas_Length; i++) {
                    for (int j = 0; j < mas_Length; j++) {
                        mas[i, j] = elements[i, j];
                    }
                }
            }

            public bool Get_Successful {
                get { return !Check_Error; }
            } //Возвращает корректность данных
            public string Get_Matrix {
                get { return mas_str; }
            } //Возвращает строку с матрицей пользователя
            public double[,] Get_Mas_Matrix {
                get { return mas; }
            } //Возвращает матрицу пользователя

            public double Gauss_Method() {
                int swap = 0;
                double det = 1;

                for (int i = 0; i < mas.GetLength(0); i++) {
                    det *= mas[i, i];
                    if (mas[i, i] == 0) {
                        for (int k = i + 1; k < mas.GetLength(0); k++) {
                            if (mas[k, i] != 0) {
                                double tmp;
                                for (int j = 0; j < mas.GetLength(0); j++) {
                                    tmp = mas[i, j];
                                    mas[i, j] = mas[k, j];
                                    mas[k, j] = tmp;
                                }
                                swap++;
                                break;
                            }
                            //Если не нашел строки без 0, значит система не имеет решений
                            if (k == mas.GetLength(0) - 1) {
                                Check_Error = true;
                                return -1;
                            }
                        }
                    }

                    //Деление строки на диагональный элемент
                    double diag_el = mas[i, i];
                    for (int j = 0; j < mas.GetLength(0); j++) {
                        mas[i, j] /= diag_el;
                    }

                    //Обнуление столбца под диагональным элементом
                    for (int k = i + 1; k < mas.GetLength(0); k++) {
                        diag_el = mas[k, i];
                        for (int j = i; j < mas.GetLength(0); j++) {
                            mas[k, j] -= mas[i, j] * diag_el;
                        }
                    }
                }

                return det;
            } //Метод Гаусса для поиска определителя
        }
        private class Inverse_Matrix {

            private readonly double[,] mas = null;
            private int mas_Length;
            private string mas_str = "";
            private double mas_determinant;

            private readonly double[,] inv_mas = null;
            private string inv_mas_str = "";

            private bool Check_Error = false;

            public Inverse_Matrix(string[] elements) {
                double root = Math.Round(Math.Sqrt(elements.Length), 0);
                if (root * root == elements.Length)
                    mas_Length = Convert.ToInt32(Math.Round(Math.Sqrt(elements.Length), 0));
                else
                    Check_Error = true;

                if (!Check_Error) {
                    mas = new double[mas_Length, mas_Length];
                    inv_mas = new double[mas_Length, mas_Length];
                    for (int i = 0, ind = 0; i < mas_Length; i++) {
                        mas_str += "| ";
                        for (int j = 0; j < mas_Length; j++, ind++) {
                            if (Double.TryParse(elements[ind], out mas[i, j])) {
                                mas_str += elements[ind] + " ";
                            }
                            else {
                                mas_str += "! ";
                                Check_Error = true;
                            }
                        }
                        mas_str += "|\n";
                    }
                }


            }

            public bool Set_Successful {
                set { Check_Error = !value; }
            } //Устанавливает корректность данных
            public bool Get_Successful {
                get { return !Check_Error; }
            } //Возвращает корректность данных
            public string Get_Matrix {
                get { return mas_str; }
            } //Возвращает строку с матрицей пользователя
            public double[,] Get_Mas_Matrix {
                get { return mas; }
            } //Возвращает матрицу пользователя
            public string Get_Inverse_Matrix {
                get { return inv_mas_str; }
            } //Возвращает строку с обратной матрицей

            private double Dop_A(int row, int col) {
                double[,] dop = new double[mas_Length - 1, mas_Length - 1];
                for (int i = 0, dop_i = 0; i < mas_Length; i++, dop_i++) {
                    if (i == row) {
                        dop_i--;
                        continue;
                    }
                    for (int j = 0, dop_j = 0; j < mas_Length; j++, dop_j++) {
                        if (j == col) {
                            dop_j--;
                            continue;
                        }
                        dop[dop_i, dop_j] = mas[i, j];
                    }
                }


                Determinant tmp_det = new Determinant(dop);

                return tmp_det.Gauss_Method();
            }
            public bool Create_Inverse_Matrix() {
                Determinant tmp_det = new Determinant(mas);
                mas_determinant = tmp_det.Gauss_Method();

                if (mas_determinant != 0) {
                    for (int i = 0; i < mas_Length; i++) {
                        for (int j = 0; j < mas_Length; j++) {
                            inv_mas[j, i] = Math.Pow(-1, i + j) * Dop_A(i, j) / mas_determinant;
                        }
                    }
                    for (int i = 0; i < mas_Length; i++) {
                        inv_mas_str += "| ";
                        for (int j = 0; j < mas_Length; j++) {
                            inv_mas_str += Math.Round(inv_mas[i, j], 3).ToString() + " ";
                        }
                        inv_mas_str += "|\n";
                    }
                    return true;
                }
                else {
                    return false;
                }
            } //Создает обратную матрицу
        }
        private class System_of_Equations {
            private double[,] Matrix_Gauss = null;
            private double[] Matrix_f = null;

            private int mas_Length;
            private string Matrix_str = "";

            private bool Check_Error = false;

            public System_of_Equations(string[] mat, string[] mat_f) {
                double root = Math.Round(Math.Sqrt(mat.Length), 0);
                if (root * root == mat.Length)
                    mas_Length = Convert.ToInt32(Math.Round(Math.Sqrt(mat.Length), 0));
                else
                    Check_Error = true;
                if (mat_f.Length != root)
                    Check_Error = true;

                if (!Check_Error) {
                    Matrix_Gauss = new double[mas_Length, mas_Length];
                    Matrix_f = new double[mas_Length];
                    for (int i = 0, ind = 0; i < mas_Length; i++) {
                        Matrix_str += "| ";
                        for (int j = 0; j < mas_Length; j++, ind++) {
                            if (Double.TryParse(mat[ind], out Matrix_Gauss[i, j])) {
                                Matrix_str += mat[ind] + " ";
                            }
                            else {
                                Matrix_str += "! ";
                                Check_Error = true;
                            }
                        }
                        Matrix_str += "| ";
                        if (Double.TryParse(mat_f[i], out Matrix_f[i])) {
                            Matrix_str += mat_f[i] + " ";
                        }
                        else {
                            Matrix_str += "! ";
                            Check_Error = true;
                        }
                        Matrix_str += "|\n";
                    }
                }
            }

            public bool Get_Successful {
                get { return !Check_Error; }
            }
            public string Get_System {
                get { return Matrix_str; }
            }

            public double[] Get_Roots() {
                double[] Res_x = new double[Matrix_Gauss.GetLength(0)];

                ///////////////////////////////////Прямой ход
                for (int i = 0; i < Matrix_Gauss.GetLength(0) - 1; i++) {
                    if (Matrix_Gauss[i, i] == 0) {
                        for (int k = i + 1; k < Matrix_Gauss.GetLength(0); k++) {
                            if (Matrix_Gauss[k, i] != 0) {
                                double tmp;
                                for (int j = 0; j < Matrix_Gauss.GetLength(0); j++) {
                                    tmp = Matrix_Gauss[i, j];
                                    Matrix_Gauss[i, j] = Matrix_Gauss[k, j];
                                    Matrix_Gauss[k, j] = tmp;
                                }
                                break;
                            }
                            //Если не нашел строки без 0, значит система не имеет решений
                            if (k == Matrix_Gauss.GetLength(0) - 1)
                                Check_Error = true;
                        }
                    }

                    //Если система не имеет решений, заканчиваем прямой ход
                    if (Check_Error)
                        break;

                    //Деление строки на диагональный элемент
                    double Diag_el = Matrix_Gauss[i, i];
                    Matrix_f[i] /= Diag_el;
                    for (int j = 0; j < Matrix_Gauss.GetLength(0); j++) {
                        Matrix_Gauss[i, j] /= Diag_el;
                    }
                    //Обнуление столбца под диагональным элементом
                    for (int k = i + 1; k < Matrix_Gauss.GetLength(0); k++) {
                        double Diag_el_k = Matrix_Gauss[k, i];
                        Matrix_f[k] -= Matrix_f[i] * Diag_el_k;
                        for (int j = i; j < Matrix_Gauss.GetLength(0); j++) {
                            Matrix_Gauss[k, j] -= Matrix_Gauss[i, j] * Diag_el_k;
                        }
                    }
                }
                Matrix_f[Matrix_Gauss.GetLength(0) - 1] /= Matrix_Gauss[Matrix_Gauss.GetLength(0) - 1, Matrix_Gauss.GetLength(0) - 1];
                Matrix_Gauss[Matrix_Gauss.GetLength(0) - 1, Matrix_Gauss.GetLength(0) - 1] = 1;

                ///////////////////////////////////Обратный ход
                Res_x[Matrix_Gauss.GetLength(0) - 1] = Matrix_f[Matrix_Gauss.GetLength(0) - 1];
                for (int i = Matrix_Gauss.GetLength(0) - 2; i > -1; i--) {
                    for (int j = Matrix_Gauss.GetLength(0) - 1; j > i; j--) {
                        Matrix_f[i] -= Matrix_Gauss[i, j] * Res_x[j];
                    }
                    Res_x[i] = Matrix_f[i];
                }

                return Res_x;
            }
        }
        private struct Theorem {
            public string name { get; }
            public string autor { get; }
            public string path { get; }
            public Theorem(string n, string a, string p) {
                name = n;
                autor = a;
                path = p;
            }
        }
        private List<Theorem> List_of_Theorems = new List<Theorem>();
        private string T_List = "";

        public Mathematics() {
            var directory = new DirectoryInfo(Global.Math_Data_Path);

            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (DirectoryInfo dir in dirs) {
                List_of_Theorems.Add(new Theorem(dir.Name.Substring(0, dir.Name.LastIndexOf('.')), dir.Name.Substring(dir.Name.LastIndexOf('.') + 2), dir.FullName));
                T_List += " - " + dir.Name + "\n";
            }

        }

        public string Get_LT {
            get {
                return T_List;
            }
        } //Возвращает список теорем

        public string Search_Theorem(string T_name) {
            string theorem = "Теорема не найдена.";
            foreach (Theorem th in List_of_Theorems) {
                if (th.name.ToLower() == T_name) {
                    theorem = th.path;
                    break;
                }
            }

            return theorem;
        } //Ищет теорему по названию
        public string Search_Autor(string A_name) {
            string res = "";
            foreach (Theorem th in List_of_Theorems) {
                if (th.autor.ToLower() == A_name) {
                    res += " - " + th.name + "\n";
                }
            }

            if (res == "") {
                res = "Автор не найден.";
            }

            return res;
        } //Ищет список теорем данного ученого
        public string Search_Determinant(string mat) {
            Determinant New_Mat = new Determinant(mat.Split(' '));

            string _result = "";

            if (New_Mat.Get_Mas_Matrix != null) {
                _result += New_Mat.Get_Matrix;
            }
            else {
                _result += "Ошибка! Неверная размерность матрицы";
            }

            _result += "\nОпределитель ";
            if (New_Mat.Get_Successful) {
                double _determinant = New_Mat.Gauss_Method();
                _result += "равен: " + Math.Round(_determinant, 3).ToString();
                return _result;
            }
            else {
                _result += "не определен.";
                return _result;
            }
        } //Нахождение определителя по матрице
        public string Search_Inverse_Matrix(string mat) {
            Inverse_Matrix New_Mat = new Inverse_Matrix(mat.Split(' '));

            string _result = "";

            if (New_Mat.Get_Mas_Matrix != null) {
                _result += New_Mat.Get_Matrix;
                New_Mat.Set_Successful = New_Mat.Create_Inverse_Matrix();
            }
            else {
                _result += "Ошибка! Неверная размерность матрицы";
            }

            if (New_Mat.Get_Successful) {
                _result += "\nОбратная матрица:\n\n";
                _result += New_Mat.Get_Inverse_Matrix;
                return _result;
            }
            else {
                _result += "Обратной матрицы не существует.";
                return _result;
            }
        } //Нахождение обратной матрицы
        public string Search_Roots_System_(string sys, string ans) {
            string _result = "";
            System_of_Equations New_Sys = new System_of_Equations(sys.Split(' '), ans.Split(' '));
            if (New_Sys.Get_Successful) {
                _result += "Система:\n" + New_Sys.Get_System + "\n";
                double[] roots = New_Sys.Get_Roots();
                if (New_Sys.Get_Successful) {
                    _result += "Корни системы: ";
                    for (int i = 0; i < roots.Length; i++) {
                        _result += Math.Round(roots[i], 3).ToString() + " ";
                    }
                    return _result;
                }
                else {
                    _result += "Не имеет решений, либо их бесконечно много.";
                    return _result;
                }
            }
            else {
                _result += "Система записана некорректно";
                return _result;
            }
        }
    }
}
