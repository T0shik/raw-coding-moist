import 'package:moist_store/internals/section_state.dart';

class SectionContainer<T> {
  SectionState state;
  T data;

  SectionContainer(this.state, this.data);

  static SectionContainer<T> loading<T>({T data}) =>
      new SectionContainer(SectionState.Loading, data);

  static SectionContainer<T> reject<T>({T data}) =>
      new SectionContainer(SectionState.Rejected, data);

  static SectionContainer<T> accept<T>({T data}) =>
      new SectionContainer(SectionState.Accepted, data);
}