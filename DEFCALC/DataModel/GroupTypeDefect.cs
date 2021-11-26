using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DEFCALC.DataModel
{
  public  class GroupTypeDefect
    {
      public ObservableCollection<Dict> DictListTypeDefect { get; private set; }

      public GroupTypeDefect()
      {
          DictListTypeDefect = new ObservableCollection<Dict>();
          DictListTypeDefect.Add(
                          new Dict("коррозия", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("выбоина", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("вмятина", "дефект формы")
                          );
          DictListTypeDefect.Add(
                          new Dict("продольная трещина", "трещиноподобный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("язва", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("каверна", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("окружная коррозия", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("глобальная коррозия", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("продольная канавка", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("поперечная канавка", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("зона продольных трещин", "трещиноподобный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("царапина", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("задир", "коррозионный дефект")
                          );
          DictListTypeDefect.Add(
                          new Dict("вмятина с коррозией", "дефект формы")
                          );
          DictListTypeDefect.Add(
                          new Dict("вмятина в сварном шве", "дефект формы")
                          );
          DictListTypeDefect.Add(
                          new Dict("гофра", "дефект формы")
                          );
          DictListTypeDefect.Add(
                          new Dict("аномальный шов", "аномалия шва")
                          );
          DictListTypeDefect.Add(
                          new Dict("аномальный продольный шов", "аномалия шва")
                          );
          DictListTypeDefect.Add(
                          new Dict("аномальный поперечный шов", "аномалия шва")
                          );
          DictListTypeDefect.Add(
                          new Dict("нетипизированный дефект", "нерасчетные аномалии")
                          );
          DictListTypeDefect.Add(
                          new Dict("технологический дефект металла", "нерасчетные аномалии")
                          );
          DictListTypeDefect.Add(
                          new Dict("металл снаружи", "нерасчетные аномалии")
                          );
          DictListTypeDefect.Add(
                         new Dict("не определен", "нерасчетные аномалии")
                         );
          DictListTypeDefect.Add(
                         new Dict("стресс-коррозия", "стресс-коррозия")
                         );
          DictListTypeDefect.Add(
                         new Dict("водородное растрескивание", "стресс-коррозия")
                         );
          DictListTypeDefect.Add(
                         new Dict("вмятина с потерей металла", "дефект формы")
                         );
          DictListTypeDefect.Add(
                         new Dict("дуговой пережег", "нерасчетные аномалии")
                         );
          DictListTypeDefect.Add(
                         new Dict("искусственный дефект", "нерасчетные аномалии")
                         );
          DictListTypeDefect.Add(
                         new Dict("коррозионный кластер", "коррозионный дефект")
                         );
          DictListTypeDefect.Add(
                         new Dict("овальность", "дефект формы")
                         );
          DictListTypeDefect.Add(
                         new Dict("отслаивание", "нерасчетные аномалии")
                         );
          DictListTypeDefect.Add(
                         new Dict("продольная риска", "коррозионный дефект")
                         );
          DictListTypeDefect.Add(
                         new Dict("поперечная риска", "коррозионный дефект")
                         );
          DictListTypeDefect.Add(
                         new Dict("расслоение", "нерасчетные аномалии")
                         );
          DictListTypeDefect.Add(
                         new Dict("трещина поперечного шва", "трещиноподобный дефект")
                         );
          DictListTypeDefect.Add(
                         new Dict("трещина продольного шва", "трещиноподобный дефект")
                         );
          DictListTypeDefect.Add(
                        new Dict("трещина спирального шва", "трещиноподобный дефект")
                        );
          DictListTypeDefect.Add(
                         new Dict("питтинговая коррозия", "коррозионный дефект")
                         );

      }

      public string GetGroupTypeDefect(string nameDefect)
      {
          string nameGroup = "нерасчетные аномалии";

          foreach (var defectType in DictListTypeDefect)
          {
               if (defectType.Key == nameDefect)
               {
                   nameGroup = defectType.Name;
                   break;
               }
          }
          return nameGroup;
      }
    }
}
